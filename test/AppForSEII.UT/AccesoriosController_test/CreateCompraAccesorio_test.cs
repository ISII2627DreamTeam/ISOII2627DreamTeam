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
        // Clase de pruebas unitarias (UT) para el método POST de CompraAccesorios.
        // Hereda de AppForSEII4SqliteUT para usar una base de datos en memoria SQLite y aislar las pruebas.
        public CreateCompraAccesorio_test()
        {
            // 1. CONSTRUCTOR DE LA CLASE DE TEST (Configuración del Escenario)
            // Este código se ejecuta ANTES de cada uno de los métodos [Fact]. 
            // Sirve para llenar el almacén virtual con datos iniciales para poder probar.
            var accesorio1 = new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m);
            var accesorio2 = new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m);
            
            _context.AddRange(accesorio1, accesorio2);
            _context.SaveChanges();
        }
        
        // La compra se realiza con éxito, se devuelve un 201 Created y se actualiza el stock de los accesorios comprados.
        [Fact]
        public async Task CreateCompraAccesorio_ReturnsCreated_AndUpdatesStock()
        {
            // ARRANGE (Preparar)
            // Usamos 'Moq' para simular el ILogger, ya que en un test no nos interesa escribir en consola real.
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            // Creamos un DTO de entrada. El cliente pide 3 unidades del accesorio con ID 2.
            var lineasDto = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(2, 3) };
            var nuevaCompraDto = new CompraAccesorioDTO("Jose Miguel", "Martinez", "Avenida Avenidosa 456", "777888999", MetodoPago.PayPal, lineasDto, 0m);

            // ACT (Actuar)
            // Llamamos al método POST del controlador simulando la petición del usuario.
            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            // ASSERT (Comprobar)
            // 1. Comprobamos que el controlador devolvió HTTP 201 Created.
            var createdResult = Assert.IsType<CreatedAtActionResult>(result); 
            // 2. Comprobamos que el cuerpo de la respuesta es un DTO.
            var resultDto = Assert.IsType<CompraAccesorioDTO>(createdResult.Value);
            
            // 3. Creamos el DTO "esperado". Simulamos cómo debería quedar después de calcular precios.
            // 3 unidades a 15.50€ cada una = 46.50€.
            var lineasEsperadas = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(2, "Cama de Cristal", 3, 15.50m, 46.50m) };
            // Copiamos el Id y FechaCompra que generó el servidor para que coincidan exactamente en el test.
            var compraEsperada = new CompraAccesorioDTO(resultDto.Id, resultDto.FechaCompra, "Jose Miguel", "Martinez", "Avenida Avenidosa 456", "777888999", MetodoPago.PayPal, lineasEsperadas, 46.50m);

            // 4. Comprobamos que el DTO esperado y el que devolvió la API son idénticos (usa nuestro método Equals sobrescrito).
            Assert.Equal(compraEsperada, resultDto); 
            
            // 5. Comprobamos la lógica de negocio (resta de stock). 
            // Había 20 camas de cristal, se compraron 3, deben quedar 17.
            var accesorioActualizado = await _context.Accesorios.FindAsync(2);
            Assert.Equal(17, accesorioActualizado!.CantidadDisponible); 
        }

        // Falla porque el carrito está vacío
        [Fact]
        public async Task CreateCompraAccesorio_ReturnsBadRequest_WhenCarritoVacio()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            // ARRANGE: Pasamos una lista de líneas vacía.
            var lineasVacias = new List<LineaCompraAccesorioDTO>();
            var nuevaCompraDto = new CompraAccesorioDTO("Pedro", "Lopez", "Calle Falsa 123", "111222333", MetodoPago.Bizum, lineasVacias, 0m);

            // ACT
            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            // ASSERT: Comprobamos que el controlador cortó la ejecución y devolvió HTTP 400.
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        // Falla por falta de stock
        [Fact]
        public async Task CreateCompraAccesorio_ReturnsBadRequest_WhenNotEnoughStock()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            // ARRANGE: El cliente pide 50 unidades, pero en el almacén virtual (arriba) solo hay 20.
            var lineasSinStock = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(2, 50) };
            var nuevaCompraDto = new CompraAccesorioDTO("Maria", "Gomez", "Avenida Siempreviva 742", "987654321", MetodoPago.Bizum, lineasSinStock, 0m);

            // ACT
            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);
            
            // ASSERT: Devuelve HTTP 400.
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        public async Task CreateCompraAccesorio_ReturnsBadRequest_WhenAccesorioDoesNotExist()
        {
            var mockLogger = new Mock<ILogger<CompraAccesoriosController>>();
            var controller = new CompraAccesoriosController(_context, mockLogger.Object);

            // ARRANGE: El cliente intenta comprar el accesorio ID 999, que no hemos metido en nuestra BD SQLite.
            var lineasInexistentes = new List<LineaCompraAccesorioDTO> { new LineaCompraAccesorioDTO(999, 1) };
            var nuevaCompraDto = new CompraAccesorioDTO("Luis", "Martinez", "Calle Falsa 123", "123456789", MetodoPago.TarjetaCredito, lineasInexistentes, 0m);

            // ACT
            var result = await controller.CreateCompraAccesorio(nuevaCompraDto);

            // ASSERT: Devuelve HTTP 400.
            Assert.IsType<BadRequestObjectResult>(result); 
        }
    }
}