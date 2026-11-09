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
    [Route("api/[controller]")] //Define la ruta base y activa las características de API Controller (validación automática, etc)
    [ApiController]
    public class AccesoriosController : ControllerBase 
    {
        private readonly ApplicationDbContext _context; //herramienta para acceder a la base de datos
        private readonly ILogger<AccesoriosController> _logger; //herramienta para registrar información de lo que ocurre en la aplicación

        public AccesoriosController(ApplicationDbContext context, ILogger<AccesoriosController> logger)
        {
            _context = context; //asignamos el contexto de la base de datos a la variable privada _context
            _logger = logger; //asignamos el logger a la variable privada _logger
        }

        [HttpGet] // Indica que este método responde a solicitudes HTTP GET (solo lectura)
        [Route("[action]")] //la ruta de la acción será /api/Accesorios/GetAccesorios
        [ProducesResponseType(typeof(IList<AccesorioDTO>), (int)HttpStatusCode.OK)] //Si se encuentra la compra, devuelve un 200 OK y el DTO
        
        public async Task<IActionResult> GetAccesorios(string? nombre, string? categoria) //el cliente puede enviar ambos parametros o ninguno, si no envía ninguno devuelve todos los accesorios
        {
            return Ok(await _context.Accesorios
                // Parte A (Nombre): Si la variable 'nombre' viene vacía, esta primera mitad da TRUE y deja pasar todos los registros.
                // Si trae texto, ejecuta la segunda mitad y comprueba si el nombre del accesorio en la BD contiene (.Contains) ese texto.
                .Where(a => (string.IsNullOrEmpty(nombre) || a.Nombre.Contains(nombre)) &&
                            //Parte B (Categoría): Si la variable 'categoria' viene vacía, esta primera mitad da TRUE y deja pasar todos los registros.
                            // Si trae texto, ejecuta la segunda mitad y comprueba si la categoría del accesorio en la BD es igual a la categoría enviada por el cliente.
                            (string.IsNullOrEmpty(categoria) || a.Categoria.ToString() == categoria)) 
                .Select(a => new AccesorioDTO(a)) //convertimos cada entidad original a en un DTO para enviarlo al cliente, evitando enviar información sensible o innecesaria
                .ToListAsync()); //Se traduce todo lo anterior a una consulta SQL y se ejecuta de forma asíncrona, devolviendo una lista de AccesorioDTO
        }
    }
}