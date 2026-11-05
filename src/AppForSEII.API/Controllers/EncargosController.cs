using AppForSEII.API.Data;
using AppForSEII.API.DTOs.EncargoImpresionDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII.API.DTOs.EncargoParaCrearDTOs;

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

        // POST: api/Encargos
    [HttpPost]
    public async Task<ActionResult<EncargoParaCrearDTO>> PostEncargoImpresion(EncargoParaCrearDTO encargoDTO)
    {
        if (encargoDTO == null || encargoDTO.LineasEncargo == null || !encargoDTO.LineasEncargo.Any())
        {
            return BadRequest("El encargo debe contener al menos una línea.");
        }

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

        decimal precioTotal = 0;

        foreach (var lineaDTO in encargoDTO.LineasEncargo)
        {
            var pieza = await _context.Piezas3D
                .Include(p => p.MaterialesValidos)
                .FirstOrDefaultAsync(p => p.Id == lineaDTO.PiezaId);

            if (pieza == null)
            {
                return BadRequest($"La pieza con ID {lineaDTO.PiezaId} no existe.");
            }

            var material = await _context.Materiales
                .FirstOrDefaultAsync(m => m.Id == lineaDTO.MaterialId);

            if (material == null)
            {
                return BadRequest($"El material con ID {lineaDTO.MaterialId} no existe.");
            }

            decimal precioUnidad = pieza.Peso * material.PrecioPorGramo;
            decimal subtotal = precioUnidad * lineaDTO.Cantidad;
            precioTotal += subtotal;

            var lineaEncargo = new LineaEncargo
            {
                PiezaId = pieza.Id,
                MaterialId = material.Id,
                Cantidad = lineaDTO.Cantidad,
                PrecioUnidad = precioUnidad,
                Subtotal = subtotal
            };

            encargo.LineasEncargo.Add(lineaEncargo);
        }

        encargo.PrecioTotal = precioTotal;

        _context.EncargosImpresion.Add(encargo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(PostEncargoImpresion), new { id = encargo.Id }, encargoDTO);
    }
    }
}