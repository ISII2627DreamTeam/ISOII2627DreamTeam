using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using System.Net; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging; 
using AppForSEII.API.DTOs.CompraAccesoriosDTOs; 

namespace AppForSEII.API.Controllers 
{ 
    [Route("api/[controller]")] 
    [ApiController] 
    public class CompraAccesoriosController : ControllerBase 
    { 
        // 1. Herramientas para trabajar
        // _context es nuestro acceso al almacén (la base de datos)
        //_logger es nuestra herramienta para registrar información de lo que ocurre en la aplicación
        //Para ello podemos usar _logger.LogInformation o _logger.LogWarning, etc
        private readonly ApplicationDbContext _context; 
        private readonly ILogger<CompraAccesoriosController> _logger; 

        // 2. Constructor de la clase
        // El ASP.NET Core se encarga de inyectar las dependencias (contexto y logger) cuando se crea una instancia del controlador
        // -context: nos permite acceder a la base de datos
        // -logger: nos permite registrar información de lo que ocurre en la aplicación
        // si se requiere de otro servicio, se puede agregar como parámetro en el constructor y el framework se encargará de inyectarlo
        public CompraAccesoriosController(ApplicationDbContext context, ILogger<CompraAccesoriosController> logger) 
        { 
            _context = context; 
            _logger = logger; 
        } 

        // 3. Métodos de acción del controlador
        [HttpGet] // Indica que este método responde a solicitudes HTTP GET (solo lectura)
        [Route("[action]")] //la ruta de la acción será /api/CompraAccesorios/GetCompraAccesorio/{id}
        [ProducesResponseType(typeof(CompraAccesorioDTO), (int)HttpStatusCode.OK)]  //Si se encuentra la compra, devuelve un 200 OK y el DTO
        [ProducesResponseType((int)HttpStatusCode.NotFound)] //Si no se encuentra la compra, devuelve un 404 Not Found
        public async Task<IActionResult> GetCompraAccesorio(int id) 
        {   
            //Se busca la base de datos de forma asíncrona
            var compraDto = await _context.ComprasAccesorios
                .Include(c => c.LineasCompra) // traer las líneas de compra asociadas a la compra
                    .ThenInclude(l => l.Accesorio) // traer los accesorios asociados a cada línea de compra
                .Where(c => c.Id == id) //traer solo la compra con el id especificado por el cliente
                .Select(c => new CompraAccesorioDTO(c)) //Tomamos los datos de la compra y los convertimos a un DTO para enviarlos al cliente
                .FirstOrDefaultAsync(); //coge el primer elemento de la lista o null si no hay elementos (todo de forma asincrona para que el flujo siga)

            if (compraDto == null) //Si la consulta FirstOrDefaultAsync no encontro registro devuelve null
            { 
                _logger.LogWarning($"No se encontró la compra con ID: {id}"); //registramos un aviso
                return NotFound(); //cortamos ejecución enviando un 404 Not Found al cliente
            } 

            return Ok(compraDto); //si no era nulo la eejecucion llega hasta aquí y devolvemos un 200 OK con el DTO de la compra
        }

        // 4. Método para crear una nueva compra de accesorios
        [HttpPost] // Indica que este método responde a solicitudes HTTP POST (creación de recursos)
        [Route("[action]")] //La URL termina en /api/CompraAccesorios/CreateCompraAccesorio
        [ProducesResponseType(typeof(CompraAccesorioDTO), (int)HttpStatusCode.Created)] //Exito devuelve 201 Created con el DTO de la compra creada
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] //Error devuelve 400
        public async Task<IActionResult> CreateCompraAccesorio(CompraAccesorioDTO compraDto)
        {
            if (compraDto.Lineas == null || !compraDto.Lineas.Any()) // Comprobamos si el cliente ha enviado al menos una línea de compra
            {
                ModelState.AddModelError("Lineas", "La compra debe incluir al menos un accesorio."); //Añadimos el error al diccionario ModelState, que guarda los errores de validación
            }

            //MODIFICACION EXAMEN
            if(compraDto.DireccionEnvio.Contains("Apartado"))
            {
                ModelState.AddModelError("DireccionEnvio", "¡Error! No se realizan envíos a apartados de correos");
            }

            if (ModelState.ErrorCount > 0) // Si hay errores de validación, devolvemos un 400 Bad Request con los detalles de los errores
            {
                return BadRequest(new ValidationProblemDetails(ModelState)); //Devolvemos un 400 Bad Request con los detalles de los errores de validación
            }

            //Instanciamos el modelo de dominio
            // - Pasamos 0 como id porque la base de datos lo generará automáticamente
            // - Pasamos DateTime.Now como fecha de compra porque queremos registrar la fecha actual
            var nuevaCompra = new CompraAccesorios( 
                compraDto.NombreCliente, 
                compraDto.ApellidosCliente, 
                compraDto.DireccionEnvio, 
                compraDto.NumeroTelefono, 
                DateTime.Now, 
                compraDto.MetodoPago, 
                0m //Inicializamos el precio total a 0, se calculará después de procesar las líneas de compra
            );

            decimal precioTotalCalculado = 0;

            //Iteramos sobre la lista de lineas que viene en el DTO que sabemos que no es nula porque lo comprobamos antes
            foreach (var lineaDto in compraDto.Lineas!)
            {
                // Busca el producto real en la Base de Datos usando su ID.
                var accesorio = await _context.Accesorios.FindAsync(lineaDto.AccesorioId);
                
                // VALIDACIÓN A: Comprueba que el producto exista.
                if (accesorio == null)
                {
                    ModelState.AddModelError("Accesorio", $"El accesorio con id {lineaDto.AccesorioId} no existe.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                // VALIDACIÓN B: Comprueba que haya suficiente stock disponible para la cantidad solicitada.
                if (accesorio.CantidadDisponible < lineaDto.Cantidad)
                {
                    ModelState.AddModelError("Stock", $"No hay stock suficiente para el accesorio {accesorio.Nombre}.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                // Crea la línea de compra real. IMPORTANTE: Usa el precio de la base de datos (accesorio.Precio), no del cliente.
                var nuevaLinea = new LineaCompraAccesorio(accesorio, nuevaCompra, lineaDto.Cantidad, accesorio.Precio);
                
                // Añade la línea a la compra y descuenta el stock del inventario.
                nuevaCompra.LineasCompra.Add(nuevaLinea);
                
                accesorio.CantidadDisponible -= lineaDto.Cantidad;
                precioTotalCalculado += nuevaLinea.Subtotal;// Acumulamos el subtotal de la línea al precio total calculado
            }

            nuevaCompra.PrecioTotal = precioTotalCalculado; // Asignamos el precio total calculado a la compra

            _context.ComprasAccesorios.Add(nuevaCompra); // Agregamos la nueva compra al contexto para que se guarde en la base de datos
            await _context.SaveChangesAsync(); // Guardamos los cambios en la base de datos de forma asíncrona

            var compraResultDto = new CompraAccesorioDTO(nuevaCompra); // Creamos un DTO de la compra recién creada para devolverlo al cliente

            return CreatedAtAction(nameof(GetCompraAccesorio), new { id = nuevaCompra.Id }, compraResultDto); // Devolvemos un 201 Created con la ubicación de la nueva compra y el DTO de la compra creada
        }
    } 
}