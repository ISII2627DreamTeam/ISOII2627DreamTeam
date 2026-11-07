using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasImpresoraController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        
        private readonly ILogger<ReservasImpresoraController> _logger;

        public ReservasImpresoraController(ApplicationDbContext context, ILogger<ReservasImpresoraController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Impresora3DDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetImpresorasParaReservar(string? nombre, string? tipo)
        {
            return Ok(await _context.Impresora3Ds
                .Where(i => (string.IsNullOrEmpty(nombre) || i.Nombre.Contains(nombre)) &&
                            (string.IsNullOrEmpty(tipo) || i.Tipo.ToString() == tipo))
                .Select(i => new Impresora3DDTO(i))
                .ToListAsync());
        }
    }
}