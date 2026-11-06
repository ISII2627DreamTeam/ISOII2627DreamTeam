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
        private readonly ApplicationDbContext _context; 
        private readonly ILogger<CompraAccesoriosController> _logger; 

        public CompraAccesoriosController(ApplicationDbContext context, ILogger<CompraAccesoriosController> logger) 
        { 
            _context = context; 
            _logger = logger; 
        } 

        [HttpGet] 
        [Route("[action]")] 
        [ProducesResponseType(typeof(CompraAccesorioDetailDTO), (int)HttpStatusCode.OK)] 
        [ProducesResponseType((int)HttpStatusCode.NotFound)] 
        public async Task<IActionResult> GetCompraAccesorio(int id) 
        { 
            var compraDto = await _context.ComprasAccesorios 
                .Include(c => c.LineasCompra) 
                    .ThenInclude(l => l.Accesorio) 
                .Where(c => c.Id == id)
                // Se simplifica el mapeo llamando directamente al constructor del DTO
                .Select(c => new CompraAccesorioDetailDTO(c)) 
                .FirstOrDefaultAsync(); 

            if (compraDto == null) 
            { 
                _logger.LogWarning($"No se encontró la compra con ID: {id}"); 
                return NotFound(); 
            } 

            return Ok(compraDto); 
        } 

        // POST AQUI 
    } 
}