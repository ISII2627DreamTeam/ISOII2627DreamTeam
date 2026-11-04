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
    public class AccesoriosController : ControllerBase 
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccesoriosController> _logger;

        public AccesoriosController(ApplicationDbContext context, ILogger<AccesoriosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")] 
        [ProducesResponseType(typeof(IList<AccesorioDTO>), (int)HttpStatusCode.OK)]
        
        public async Task<IActionResult> GetAccesorios(string? nombre, string? categoria)
        {
            return Ok(await _context.Accesorios
                .Where(a => (string.IsNullOrEmpty(nombre) || a.Nombre.Contains(nombre)) &&
                            (string.IsNullOrEmpty(categoria) || a.Categoria.ToString() == categoria))
                .Select(a => new AccesorioDTO(a))
                .ToListAsync());
        }
    }
}