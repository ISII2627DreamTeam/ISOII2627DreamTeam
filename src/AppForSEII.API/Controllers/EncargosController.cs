using AppForSEII.API.Data;
using AppForSEII.API.DTOs.EncargoImpresionDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII.API.DTOs.EncargoParaCrearDTOs;
using AppForSEII.API.DTOs.PiezaDetalleDTOs;

namespace AppForSEII.API.Controllers
{
    // Define la ruta base "api/Encargos" y marca la clase como un controlador de API REST.
    [Route("api/[controller]")]
    [ApiController]
    public class EncargosController : ControllerBase
    {
        // Atributo para acceder a la base de datos a través de Entity Framework Core
        private readonly ApplicationDbContext _context;
        // Atributo para registrar errores (logs) durante la ejecución de los métodos
        private readonly ILogger<EncargosController> _logger;

        // Constructor estándar con inyección de dependencias para recibir el contexto de BD y el logger
        public EncargosController(ApplicationDbContext context, ILogger<EncargosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // PASO 3: Método GET para obtener el catálogo de piezas permitiendo filtrar opcionalmente por nombre y categoría
        // Ruta: GET api/Encargos/GetPiezasParaEncargar
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PiezaParaEncargarDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPiezasParaEncargar(string? nombre, string? categoria)
        {
            // Consulta asíncrona a la tabla Piezas3D:
            // 1. .Where(): Filtra por 'nombre' si no es nulo ni vacío, y por 'categoria' (convertida a string) si no es nula ni vacía.
            //    Si ambos parámetros llegan como null, devuelve todas las piezas de la tabla.
            // 2. .Select(): Proyecta cada entidad Pieza3D hacia el DTO PiezaParaEncargarDTO usando su constructor.
            // 3. .ToListAsync(): Ejecuta la consulta en la base de datos y devuelve la lista resultante.
            var piezas = await _context.Piezas3D
                .Where(p => (string.IsNullOrEmpty(nombre) || p.Nombre.Contains(nombre)) &&
                            (string.IsNullOrEmpty(categoria) || p.Categoria.ToString().Contains(categoria)))
                .Select(p => new PiezaParaEncargarDTO(
                    p.Id,
                    p.Nombre,
                    p.Categoria.ToString(),
                    p.Peso))
                .ToListAsync();

            // Devuelve el código de estado HTTP 200 (OK) con la lista de piezas encontradas.
            return Ok(piezas);
        }

// POST: api/Encargos
    // Método POST para crear un nuevo encargo de impresión validando todas las reglas de negocio.
    // Ruta: POST api/Encargos/PostEncargoImpresion
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(EncargoParaCrearDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<EncargoParaCrearDTO>> PostEncargoImpresion(EncargoParaCrearDTO encargoDTO)
    {
        // 1. Validar que el DTO contenga líneas de encargo (que la lista no sea nula ni esté vacía).
        // Si no tiene ninguna pieza, se registra el error en ModelState y se devuelve 400 BadRequest.
        if (encargoDTO.LineasEncargo == null || !encargoDTO.LineasEncargo.Any())
        {
            ModelState.AddModelError("LineasEncargo", "Error! El encargo debe contener al menos una pieza para imprimir.");
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        // 2. Mapear el DTO al modelo de base de datos (EncargoImpresion) asignando los datos del cliente,
        // el método de pago, la fecha actual del sistema y la lista vacía donde se añadirán las líneas.
        var encargo = new EncargoImpresion
        {
            NombreCliente = encargoDTO.NombreCliente,
            ApellidosCliente = encargoDTO.ApellidosCliente,
            DireccionEnvio = encargoDTO.DireccionEnvio,
            NumeroTelefono = encargoDTO.NumeroTelefono,
            Descripcion = encargoDTO.Descripcion,
            MetodoPago = encargoDTO.MetodoPago,
            FechaEncargo = DateTime.Now,
            LineasEncargo = new List<LineaEncargo>()
        };

        // Variable acumuladora para calcular el importe total del encargo sumando el subtotal de cada línea.
        decimal precioTotal = 0;

        // 3. Procesar cada línea validando piezas y que el material sea válido para esa pieza
        foreach (var lineaDTO in encargoDTO.LineasEncargo)
        {
            // Se busca la pieza en la base de datos por su ID, incluyendo (.Include) su lista de materiales compatibles.
            var pieza = await _context.Piezas3D
                .Include(p => p.MaterialesValidos)
                .FirstOrDefaultAsync(p => p.Id == lineaDTO.PiezaId);

            // Si la pieza no existe en la base de datos, se añade el error al ModelState y se pasa a la siguiente línea.
            if (pieza == null)
            {
                ModelState.AddModelError("PiezaId", $"Error! La pieza con ID {lineaDTO.PiezaId} no existe.");
                continue;
            }

            // Buscar el material DENTRO de los materiales válidos de la pieza
            var material = pieza.MaterialesValidos
                .FirstOrDefault(m => m.Id == lineaDTO.MaterialId);

            // Si el material no se encuentra en la lista de MaterialesValidos de esa pieza, se registra el error en ModelState.
            if (material == null)
            {
                ModelState.AddModelError("MaterialId", $"Error! El material con ID {lineaDTO.MaterialId} no es válido para la pieza '{pieza.Nombre}'.");
                continue;
            }

            // Comprobar si hay suficiente stock en gramos para imprimir la cantidad pedida (peso de la pieza * unidades)
            decimal gramosNecesarios = pieza.Peso * lineaDTO.Cantidad;
            if (material.StockGramos < gramosNecesarios)
            {
                ModelState.AddModelError("Cantidad", $"Error! No hay suficiente stock del material '{material.Nombre}' para imprimir {lineaDTO.Cantidad} unidades de '{pieza.Nombre}'.");
                continue;
            }

            // Reducir el stock del material y calcular el precio unitario, el subtotal de la línea y sumarlo al precioTotal
            material.StockGramos -= gramosNecesarios;
            decimal precioUnidad = pieza.Peso * material.PrecioPorGramo;
            decimal subtotal = precioUnidad * lineaDTO.Cantidad;
            precioTotal += subtotal;

            // Crear la entidad LineaEncargo con los datos validados y los importes calculados
            var lineaEncargo = new LineaEncargo
            {
                PiezaId = pieza.Id,
                Pieza = pieza,
                MaterialId = material.Id,
                MaterialSeleccionado = material,
                Cantidad = lineaDTO.Cantidad,
                PrecioUnidad = precioUnidad,
                Subtotal = subtotal
            };

            // Añadir la línea a la colección de líneas del encargo principal
            encargo.LineasEncargo.Add(lineaEncargo);
        }

        // Si hubo algún error de validación en las líneas durante el bucle, devolvemos 400 BadRequest sin guardar nada en BD
        if (ModelState.ErrorCount > 0)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        // Asignar el precio total acumulado a la entidad del encargo
        encargo.PrecioTotal = precioTotal;

        // 4. Guardar el nuevo encargo y sus líneas asociadas en la base de datos de forma asíncrona
        _context.EncargosImpresion.Add(encargo);
        await _context.SaveChangesAsync();

        // 5. Devolver código HTTP 201 Created junto con el ID del encargo creado y el DTO
        return CreatedAtAction(nameof(PostEncargoImpresion), new { id = encargo.Id }, encargoDTO);
    }
    // GET: api/Encargos/GetPiezaDetalle
        // Método GET para obtener todos los detalles de una pieza concreta y sus materiales compatibles a partir de su ID.
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PiezaDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPiezaDetalle(int id)
        {
            // Consulta la pieza por su ID incluyendo la relación MaterialesValidos y proyecta los datos
            // hacia PiezaDetalleDTO y su lista interna hacia MaterialParaDetalleDTO.
            var piezaDetalleDTO = await _context.Piezas3D
                .Include(p => p.MaterialesValidos)
                .Where(p => p.Id == id)
                .Select(p => new PiezaDetalleDTO(
                    p.Id,
                    p.Nombre,
                    p.Peso,
                    p.Categoria,
                    p.MaterialesValidos.Select(m => new MaterialParaDetalleDTO(
                        m.Id,
                        m.Nombre,
                        m.PrecioPorGramo,
                        m.StockGramos
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            // Si no existe ninguna pieza con ese ID (es null), registra el error en el logger y devuelve HTTP 404 NotFound.
            if (piezaDetalleDTO == null)
            {
                _logger.LogError($"Error: Pieza3D with id {id} does not exist");
                return NotFound();
            }

            // Si existe, devuelve HTTP 200 OK con el DTO detallado de la pieza.
            return Ok(piezaDetalleDTO);
        }
    }
}