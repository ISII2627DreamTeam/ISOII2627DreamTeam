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
        // Constructor: Prepara el estado inicial de la base de datos en memoria para todas las pruebas de esta clase.
        public GetCompraAccesorio_test()
        {
            // 1. Insertamos datos maestros (accesorios).
            var accesorio1 = new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m);
            var accesorio2 = new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m);
            
            _context.AddRange(accesorio1, accesorio2);
            
            // 2. Insertamos una compra de prueba con su respectiva línea asociada.
            var compraInicial = new CompraAccesorios(1, "Juan", "Perez", "Calle Falsa 123", "123456789", new DateTime(2026, 11, 8), MetodoPago.TarjetaCredito, 0m);
            var lineaInicial = new LineaCompraAccesorio(accesorio1, compraInicial, 2, accesorio1.Precio);
            
            compraInicial.LineasCompra.Add(lineaInicial);
            compraInicial.PrecioTotal = lineaInicial.Subtotal;

            _context.ComprasAccesorios.Add(compraInicial);
            _context.SaveChanges();
        }

        // Prueba: Verifica que buscar un ID válido devuelve un código 200 (OK) y los datos correctos.
        [Fact]
        public async Task GetCompraAccesorio_ReturnsOk_WhenIdExists()
        {
            // Arrange: Configuramos las dependencias (Logger falso) y creamos el controlador.
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            // Act: Ejecutamos la búsqueda simulando la petición con un ID existente (1).
            var result = await controller.GetCompraAccesorio(1);

            // Assert: Comprobamos el tipo de respuesta HTTP devuelta.
            var okResult = Assert.IsType<OkObjectResult>(result); 
            var actualDto = Assert.IsType<CompraAccesorioDTO>(okResult.Value); 
            
            // Comprobamos que los datos devueltos coinciden exactamente con los esperados.
            var lineasEsperadas = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(1, "Boquilla 0.4mm", 2, 5.99m, 11.98m) };
            var compraEsperada = new CompraAccesorioDTO(1, new DateTime(2026, 11, 8), "Juan", "Perez", "Calle Falsa 123", "123456789", MetodoPago.TarjetaCredito, lineasEsperadas, 11.98m);

            Assert.Equal(compraEsperada, actualDto);
        }

        // Prueba: Verifica que buscar un ID que no existe en la base de datos devuelve un código 404 (Not Found).
        [Fact]
        public async Task GetCompraAccesorio_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange: Preparamos el controlador.
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            // Act: Intentamos buscar un ID inexistente (999).
            var result = await controller.GetCompraAccesorio(999);

            // Assert: Verificamos que la respuesta es un HTTP 404.
            Assert.IsType<NotFoundResult>(result); 
        }
    }
}