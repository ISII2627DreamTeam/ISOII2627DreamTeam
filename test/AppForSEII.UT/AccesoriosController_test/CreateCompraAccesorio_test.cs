using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.CompraAccesoriosDTOs;

namespace AppForSEII.UT.CompraAccesoriosController_test
{
    public class CreateCompraAccesorio_test : AppForSEII4SqliteUT
    {
        public CreateCompraAccesorio_test()
        {
            var accesorio1 = new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m);
            var accesorio2 = new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m);
            
            _context.AddRange(accesorio1, accesorio2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateCompraAccesorio_ReturnsCreated_AndUpdatesStock()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            var nuevaCompraDto = new CompraAccesorioDTO
            {
                NombreCliente = "Jose Miguel",
                ApellidosCliente = "Martinez",
                DireccionEnvio = "Avenida Avenidosa 456",
                NumeroTelefono = "777888999",
                MetodoPago = MetodoPago.PayPal,
                Lineas = new List<LineaCompraAccesorioDTO>
                {
                    new LineaCompraAccesorioDTO { AccesorioId = 2, Cantidad = 3 } 
                }
            };

            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result); 
            var resultDto = Assert.IsType<CompraAccesorioDTO>(createdResult.Value);
            
            Assert.Equal(3 * 15.50m, resultDto.PrecioTotal); 
            
            var accesorioActualizado = await _context.Accesorios.FindAsync(2);
            Assert.Equal(17, accesorioActualizado!.CantidadDisponible); 
        }

        [Fact]
        public async Task CreateCompraAccesorio_ReturnsBadRequest_WhenCarritoVacio()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            var nuevaCompraDto = new CompraAccesorioDTO
            {
                NombreCliente = "Pedro",
                ApellidosCliente = "Lopez",
                DireccionEnvio = "Calle Falsa 123",
                NumeroTelefono = "111222333",
                MetodoPago = MetodoPago.Bizum,
                Lineas = new List<LineaCompraAccesorioDTO>()
            };

            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        public async Task CreateCompraAccesorio_ReturnsBadRequest_WhenNotEnoughStock()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            var nuevaCompraDto = new CompraAccesorioDTO
            {
                NombreCliente = "Maria",
                ApellidosCliente = "Gomez",
                DireccionEnvio = "Avenida Siempreviva 742",
                NumeroTelefono = "987654321",
                MetodoPago = MetodoPago.Bizum,
                Lineas = new List<LineaCompraAccesorioDTO>
                {
                    new LineaCompraAccesorioDTO { AccesorioId = 2, Cantidad = 50 } 
                }
            };

            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            Assert.IsType<BadRequestObjectResult>(result); 
        }
    }
}