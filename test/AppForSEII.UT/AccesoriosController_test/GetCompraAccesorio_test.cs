using System;
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
    public class GetCompraAccesorio_test : AppForSEII4SqliteUT
    {
        public GetCompraAccesorio_test()
        {
            var accesorio1 = new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m);
            var accesorio2 = new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m);
            
            _context.AddRange(accesorio1, accesorio2);
            
            var compraInicial = new CompraAccesorios(1, "Juan", "Perez", "Calle Falsa 123", "123456789", DateTime.Now, MetodoPago.TarjetaCredito, 0m);
            var lineaInicial = new LineaCompraAccesorio(accesorio1, compraInicial, 2, accesorio1.Precio);
            
            compraInicial.LineasCompra.Add(lineaInicial);
            compraInicial.PrecioTotal = lineaInicial.Subtotal;

            _context.ComprasAccesorios.Add(compraInicial);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCompraAccesorio_ReturnsOk_WhenIdExists()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            var result = await controller.GetCompraAccesorio(1);

            var okResult = Assert.IsType<OkObjectResult>(result); 
            var actualDto = Assert.IsType<CompraAccesorioDTO>(okResult.Value); 
            
            Assert.Equal(1, actualDto.Id);
            Assert.Single(actualDto.Lineas); 
            Assert.Equal(11.98m, actualDto.PrecioTotal); 
        }

        [Fact]
        public async Task GetCompraAccesorio_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            var result = await controller.GetCompraAccesorio(999);

            Assert.IsType<NotFoundResult>(result); 
        }
    }
}