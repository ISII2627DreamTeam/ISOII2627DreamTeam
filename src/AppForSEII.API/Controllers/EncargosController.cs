using AppForSEII.API.Data;
using AppForSEII.API.DTOs.EncargoImpresionDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncargosController : ControllerBase
    {
        // Atributo para acceder a la base de datos
        private readonly ApplicationDbContext _context;
        // Atributo para registrar errores (logs)
        private readonly ILogger<EncargosController> _logger;

        // Constructor estándar con inyección de dependencias
        public EncargosController(ApplicationDbContext context, ILogger<EncargosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // PASO 3: Método GET para obtener el catálogo de piezas
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PiezaParaEncargarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPiezasParaEncargar()
        {
            // Proyectamos de la entidad Pieza3D al DTO usando Select
            var piezas = await _context.Piezas3D
                .Select(p => new PiezaParaEncargarDTO(
                    p.Id, 
                    p.Nombre, 
                    p.Categoria.ToString(), // Convertimos el Enum a string
                    p.Peso))
                .ToListAsync();

            return Ok(piezas);
        }
    }
}