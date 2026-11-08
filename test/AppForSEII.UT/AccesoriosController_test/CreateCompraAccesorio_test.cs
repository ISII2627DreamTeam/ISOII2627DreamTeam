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

            var lineasDto = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(2, 3) };
            var nuevaCompraDto = new CompraAccesorioDTO("Jose Miguel", "Martinez", "Avenida Avenidosa 456", "777888999", MetodoPago.PayPal, lineasDto, 0m);

            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result); 
            var resultDto = Assert.IsType<CompraAccesorioDTO>(createdResult.Value);
            
            var lineasEsperadas = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(2, "Cama de Cristal", 3, 15.50m, 46.50m) };
            var compraEsperada = new CompraAccesorioDTO(resultDto.Id, resultDto.FechaCompra, "Jose Miguel", "Martinez", "Avenida Avenidosa 456", "777888999", MetodoPago.PayPal, lineasEsperadas, 46.50m);

            Assert.Equal(compraEsperada, resultDto); 
            
            var accesorioActualizado = await _context.Accesorios.FindAsync(2);
            Assert.Equal(17, accesorioActualizado!.CantidadDisponible); 
        }

        [Fact]
        public async Task CreateCompraAccesorio_ReturnsBadRequest_WhenCarritoVacio()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            var lineasVacias = new List<LineaCompraAccesorioDTO>();
            var nuevaCompraDto = new CompraAccesorioDTO("Pedro", "Lopez", "Calle Falsa 123", "111222333", MetodoPago.Bizum, lineasVacias, 0m);

            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        public async Task CreateCompraAccesorio_ReturnsBadRequest_WhenNotEnoughStock()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            var lineasSinStock = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(2, 50) };
            var nuevaCompraDto = new CompraAccesorioDTO("Maria", "Gomez", "Avenida Siempreviva 742", "987654321", MetodoPago.Bizum, lineasSinStock, 0m);

            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            Assert.IsType<BadRequestObjectResult>(result); 
        }
    }
}