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

    
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaImpresoraDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetReservaImpresora(int id)
        {
            var reserva = await _context.ReservaImpresoras
                .Include(r => r.LineasReserva)
                    .ThenInclude(l => l.Impresora)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                _logger.LogWarning($"No se encontró la reserva con ID: {id}");
                return NotFound();
            }

            return Ok(new ReservaImpresoraDTO(reserva));
        }

    
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReservaImpresoraDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateReservaImpresora(ReservaImpresoraDTO reservaDto)
        {
        
            if (reservaDto.Lineas == null || !reservaDto.Lineas.Any())
            {
                ModelState.AddModelError("Lineas", "La reserva debe incluir al menos una impresora.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var nuevaReserva = new ReservaImpresora(
                DateTime.Now,
                reservaDto.NombreCliente,
                reservaDto.ApellidosCliente,
                reservaDto.DireccionFacturacion,
                0m,
                reservaDto.MetodoPago);

            decimal precioTotal = 0;

            foreach (var lineaDto in reservaDto.Lineas)
            {
                if (!Enum.IsDefined(lineaDto.TiempoReserva))
                {
                    ModelState.AddModelError("TiempoReserva", "El tiempo de reserva no es válido.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                var impresora = await _context.Impresora3Ds.FindAsync(lineaDto.Impresora3DId);

                
                if (impresora == null)
                {
                    ModelState.AddModelError("Impresora3DId", $"La impresora con id {lineaDto.Impresora3DId} no existe.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                
                decimal subtotal = impresora.PrecioReserva * HorasDe(lineaDto.TiempoReserva);

        
                var nuevaLinea = new LineaReserva
                {
                    TiempoReserva = lineaDto.TiempoReserva,
                    PrecioSubtotal = subtotal,
                    Impresora3DId = impresora.Id,
                    Impresora = impresora,
                    ReservaImpresora = nuevaReserva
                };

                nuevaReserva.LineasReserva.Add(nuevaLinea);
                precioTotal += subtotal;
            }

            nuevaReserva.PrecioTotal = precioTotal;

            _context.ReservaImpresoras.Add(nuevaReserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservaImpresora), new { id = nuevaReserva.Id }, new ReservaImpresoraDTO(nuevaReserva));
        }

        
        private static int HorasDe(TiempoReserva tiempo) => tiempo switch
        {
            TiempoReserva.UnaHora => 1,
            TiempoReserva.DosHoras => 2,
            TiempoReserva.TresHoras => 3,
            TiempoReserva.CuatroHoras => 4,
            TiempoReserva.CincoHoras => 5,
            TiempoReserva.UnDia => 24,
            TiempoReserva.DosDias => 48,
            _ => 1
        };
    }
}