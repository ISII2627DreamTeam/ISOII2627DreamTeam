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
        [ProducesResponseType(typeof(CompraAccesorioDTO), (int)HttpStatusCode.OK)] 
        [ProducesResponseType((int)HttpStatusCode.NotFound)] 
        public async Task<IActionResult> GetCompraAccesorio(int id) 
        { 
            var compraDto = await _context.ComprasAccesorios 
                .Include(c => c.LineasCompra) 
                    .ThenInclude(l => l.Accesorio) 
                .Where(c => c.Id == id)
                .Select(c => new CompraAccesorioDTO(c)) 
                .FirstOrDefaultAsync(); 

            if (compraDto == null) 
            { 
                _logger.LogWarning($"No se encontró la compra con ID: {id}"); 
                return NotFound(); 
            } 

            return Ok(compraDto); 
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraAccesorioDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCompraAccesorio(CompraAccesorioDTO compraDto)
        {
            if (compraDto.Lineas == null || !compraDto.Lineas.Any())
            {
                ModelState.AddModelError("Lineas", "La compra debe incluir al menos un accesorio.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var nuevaCompra = new CompraAccesorios(
                0, 
                compraDto.NombreCliente, 
                compraDto.ApellidosCliente, 
                compraDto.DireccionEnvio, 
                compraDto.NumeroTelefono, 
                DateTime.Now, 
                compraDto.MetodoPago, 
                0m
            );

            decimal precioTotalCalculado = 0;

            foreach (var lineaDto in compraDto.Lineas!)
            {
                var accesorio = await _context.Accesorios.FindAsync(lineaDto.AccesorioId);
                
                if (accesorio == null)
                {
                    ModelState.AddModelError("Accesorio", $"El accesorio con id {lineaDto.AccesorioId} no existe.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                if (accesorio.CantidadDisponible < lineaDto.Cantidad)
                {
                    ModelState.AddModelError("Stock", $"No hay stock suficiente para el accesorio {accesorio.Nombre}.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                var nuevaLinea = new LineaCompraAccesorio(accesorio, nuevaCompra, lineaDto.Cantidad, accesorio.Precio);
                
                nuevaCompra.LineasCompra.Add(nuevaLinea);
                
                accesorio.CantidadDisponible -= lineaDto.Cantidad;
                precioTotalCalculado += nuevaLinea.Subtotal;
            }

            nuevaCompra.PrecioTotal = precioTotalCalculado;

            _context.ComprasAccesorios.Add(nuevaCompra);
            await _context.SaveChangesAsync();

            var compraResultDto = new CompraAccesorioDTO(nuevaCompra);

            return CreatedAtAction(nameof(GetCompraAccesorio), new { id = nuevaCompra.Id }, compraResultDto);
        }
    } 
}