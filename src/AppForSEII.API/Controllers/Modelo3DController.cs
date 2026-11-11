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
        
        // GET DETAILS: api/Modelos3D/GetModelo3D/5
        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(Modelo3DDetailDTO), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetModelo3D(int id)
        {
            // comprobamos que el contexto de base de datos no sea nulo por seguridad
            if (_context.Modelos3D == null)
            {
                _logger.LogError("Error: La tabla Modelos3D no existe en la base de datos.");
                return NotFound();
            }

            // consultamos usando Eager Loading (.Include) para traernos las licencias asociadas
            var modeloDetailDTO = await _context.Modelos3D
                .Include(m => m.Licencias) 
                .Where(m => m.Id == id) 
                .Select(m => new Modelo3DDetailDTO(
                    m.Id,
                    m.Nombre,
                    m.Categoría,
                    m.Formato,
                    m.Precio,
                    // mapeamos la lista de licencias elemento a elemento llamando a su constructor
                    m.Licencias.Select(l => new LicenciaModelo3DDTO(l.Id, l.Nombre, l.FechaExpiracion)).ToList()
                ))
                .FirstOrDefaultAsync(); 

            // si el ID no existe en la base de datos, devolvemos un 404
            if (modeloDetailDTO == null)
            {
                _logger.LogError($"Error: El modelo 3D con ID {id} no existe.");
                return NotFound();
            }

            // si todo va bien devolvemos un 200 OK con el DTO mapeado
            return Ok(modeloDetailDTO);
        }


       // POST: api/Modelos3D/PostModelo3D
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(Modelo3DDetailDTO), (int)System.Net.HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<ActionResult> PostModelo3D(Modelo3DDetailDTO modeloDTO)
        {
            // verificamos la disponibilidad del contexto de base de datos
            if (_context.Modelos3D == null)
            {
                _logger.LogError("Error: La tabla Modelos3D no existe en la base de datos.");
                return NotFound();
            }

            // el modelo debe contener al menos una licencia
            if (modeloDTO.Licencias == null || !modeloDTO.Licencias.Any())
            {
                ModelState.AddModelError("Licencias", "Debe proporcionar al menos una licencia para crear el modelo 3D.");
            }

            // comprobamos errores de validación antes de procesar la entidad
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // mapeo de DTO a entidad usando los constructores necesarios
            var modelo3D = new Modelo3D(0, modeloDTO.Nombre, modeloDTO.Categoria, modeloDTO.Formato, modeloDTO.Precio);

            var licenciasEntidad = new List<LicenciaModelo3D>();
            
            // uso de '!': garantiza al compilador que la entidad tiene su lista instanciada y no es nula para que no salte warning.
            foreach (var licenciaDTO in modeloDTO.Licencias!)
            {
                var nuevaLicencia = new LicenciaModelo3D(licenciaDTO.Nombre, licenciaDTO.FechaExpiracion, modelo3D);
                licenciasEntidad.Add(nuevaLicencia);
            }
            
            modelo3D.Licencias = licenciasEntidad;

            // se añade la entidad al contexto y persistencia en la base de datos
            _context.Modelos3D.Add(modelo3D);
            await _context.SaveChangesAsync();

            // mapeo de entidad a DTO utilizando el constructor parametrizado
            var modeloCreadoDTO = new Modelo3DDetailDTO(
                modelo3D.Id,
                modelo3D.Nombre,
                modelo3D.Categoría,
                modelo3D.Formato,
                modelo3D.Precio,
                // uso de '!': garantiza al compilador que la entidad tiene su lista instanciada y no es nula para que no salte warning.
                modelo3D.Licencias!.Select(l => new LicenciaModelo3DDTO(l.Id, l.Nombre, l.FechaExpiracion)).ToList()
            );

            // retorno indicando la ruta de acceso al nuevo recurso
            return CreatedAtAction(nameof(GetModelo3D), new { id = modelo3D.Id }, modeloCreadoDTO);
        }
    }
    
}