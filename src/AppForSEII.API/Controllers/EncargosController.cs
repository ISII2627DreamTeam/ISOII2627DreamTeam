using AppForSEII.API.Data;
using AppForSEII.API.DTOs.EncargoImpresionDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AppForSEII.API.DTOs.EncargoParaCrearDTOs;
using AppForSEII.API.DTOs.PiezaDetalleDTOs;

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
        public async Task<IActionResult> GetPiezasParaEncargar(string? nombre, string? categoria)
        {
            var piezas = await _context.Piezas3D
                .Where(p => (string.IsNullOrEmpty(nombre) || p.Nombre.Contains(nombre)) &&
                            (string.IsNullOrEmpty(categoria) || p.Categoria.ToString().Contains(categoria)))
                .Select(p => new PiezaParaEncargarDTO(
                    p.Id, 
                    p.Nombre, 
                    p.Categoria.ToString(),
                    p.Peso))
                .ToListAsync();

            return Ok(piezas);
        }

// POST: api/Encargos
    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(EncargoParaCrearDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
    public async Task<ActionResult<EncargoParaCrearDTO>> PostEncargoImpresion(EncargoParaCrearDTO encargoDTO)
    {
        // 1. Validar que el DTO contenga líneas de encargo
        if (encargoDTO.LineasEncargo == null || !encargoDTO.LineasEncargo.Any())
        {
            ModelState.AddModelError("LineasEncargo", "Error! El encargo debe contener al menos una pieza para imprimir.");
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

       // 2. Mapear el DTO al modelo de base de datos usando su constructor
        var encargo = new EncargoImpresion(
            DateTime.Now,
            encargoDTO.NombreCliente,
            encargoDTO.ApellidosCliente,
            encargoDTO.DireccionEnvio,
            encargoDTO.NumeroTelefono,
            encargoDTO.Descripcion,
            0m,
            encargoDTO.MetodoPago,
            new List<LineaEncargo>()
        );

        decimal precioTotal = 0;

        // 3. Procesar cada línea validando piezas y que el material sea válido para esa pieza
        foreach (var lineaDTO in encargoDTO.LineasEncargo)
        {
            var pieza = await _context.Piezas3D
                .Include(p => p.MaterialesValidos)
                .FirstOrDefaultAsync(p => p.Id == lineaDTO.PiezaId);

            if (pieza == null)
            {
                ModelState.AddModelError("PiezaId", $"Error! La pieza con ID {lineaDTO.PiezaId} no existe.");
                continue;
            }

            // Buscar el material DENTRO de los materiales válidos de la pieza
            var material = pieza.MaterialesValidos
                .FirstOrDefault(m => m.Id == lineaDTO.MaterialId);

            if (material == null)
            {
                ModelState.AddModelError("MaterialId", $"Error! El material con ID {lineaDTO.MaterialId} no es válido para la pieza '{pieza.Nombre}'.");
                continue;
            }

            // Comprobar si hay suficiente stock en gramos para imprimir la cantidad pedida
            decimal gramosNecesarios = pieza.Peso * lineaDTO.Cantidad;
            if (material.StockGramos < gramosNecesarios)
            {
                ModelState.AddModelError("Cantidad", $"Error! No hay suficiente stock del material '{material.Nombre}' para imprimir {lineaDTO.Cantidad} unidades de '{pieza.Nombre}'.");
                continue;
            }

            // Reducir el stock y calcular precios
            material.StockGramos -= gramosNecesarios;
            decimal precioUnidad = pieza.Peso * material.PrecioPorGramo;
            decimal subtotal = precioUnidad * lineaDTO.Cantidad;
            precioTotal += subtotal;

            var lineaEncargo = new LineaEncargo(
                lineaDTO.Cantidad,
                precioUnidad,
                subtotal,
                encargo,
                pieza,
                material
            );

            encargo.LineasEncargo.Add(lineaEncargo);
        }

        // Si hubo algún error de validación en las líneas, devolvemos BadRequest
        if (ModelState.ErrorCount > 0)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        encargo.PrecioTotal = precioTotal;

        // 4. Guardar en la base de datos
        _context.EncargosImpresion.Add(encargo);
        await _context.SaveChangesAsync();

        // 5. Devolver 201 Created
        return CreatedAtAction(nameof(PostEncargoImpresion), new { id = encargo.Id }, encargoDTO);
    }
    // GET: api/Encargos/GetPiezaDetalle
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PiezaDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPiezaDetalle(int id)
        {
            var piezaDetalleDTO = await _context.Piezas3D
                .Include(p => p.MaterialesValidos)
                .Where(p => p.Id == id)
                .Select(p => new PiezaDetalleDTO(
                    p.Id,
                    p.Nombre,
                    p.Peso,
                    p.Categoria,
                    p.MaterialesValidos.Select(m => new MaterialParaDetalleDTO(
                        m.Id,
                        m.Nombre,
                        m.PrecioPorGramo,
                        m.StockGramos
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (piezaDetalleDTO == null)
            {
                _logger.LogError($"Error: Pieza3D with id {id} does not exist");
                return NotFound();
            }

            return Ok(piezaDetalleDTO);
        }
    }
}