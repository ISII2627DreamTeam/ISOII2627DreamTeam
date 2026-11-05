using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AppForSEII.API.DTOs.CompraModelos3DDTOs;
using AppForSEII.API.Data; //pongo este using porque el archivo ApplicationDBContext está en la carpeta Data y necesito acceder a él.
using AppForSEII.API.Models; //para que encuentre las clases como Modelo3D que están en la carpeta Models.

namespace AppForSEII.API.Controllers
{
    // Define la URL base de este controlador. Al poner "[controller]", la ruta en el navegador será: /api/Modelos3D
    [Route("api/[controller]")]
    [ApiController]
    public class Modelos3DController : ControllerBase
    {
        // Variable privada para conectarnos a la base de datos de SQL Server.
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Modelos3DController> _logger;

        // Constructor: Aquí inyectamos el contexto de la base de datos (dependencias). 
        // es para que el controlador tenga permiso para leer las tablas.
        public Modelos3DController(ApplicationDbContext context, ILogger<Modelos3DController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // MÉTODO GET: Implementa el Select del catálogo
        // si tuviese que añadir un filtro nuevo por URL (ej: buscar por Formato), tengo que añadir el parámetro aquí dentro de los paréntesis con su '?' indicando que es opcional.
        [HttpGet]
        [Route("[action]")] // El "[action]" hace que la URL final incluya el nombre exacto de este método: /api/Modelos3D/GetModelos3D
        [ProducesResponseType(typeof(IList<Modelo3DDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetModelos3D(string? nombre, string? categoria)
        {
            // Accedemos a la tabla Modelos3D del contexto de forma asíncrona.
            return Ok(await _context.Modelos3D
            
                // FILTROS (WHERE): Si el cliente deja "nombre" o "categoria" en blanco, string.IsNullOrEmpty lo detecta e ignora el filtro. Si traen texto, usa Contains para buscar coincidencias.
                // Si añadiese un parámetro de búsqueda nuevo arriba, tendría que meter aquí otra línea con "&&" copiando esta misma estructura exacta.
                .Where(m => (string.IsNullOrEmpty(nombre) || m.Nombre.Contains(nombre)) &&
                            (string.IsNullOrEmpty(categoria) || m.Categoría.Contains(categoria)))
                
                // MAPEO (SELECT): Por cada entidad Modelo3D pesada que encuentra en la BD, invoca al constructor de Modelo3DDTO que creé antes para convertirlo en un objeto ligero (solo lo que le interesa para la petición).
                .Select(m => new Modelo3DDTO(m))
                
                // Ejecuta la consulta en SQL y lo convierte en una lista asíncrona para no bloquear el servidor mientras carga.
                .ToListAsync());
        }
    }
}