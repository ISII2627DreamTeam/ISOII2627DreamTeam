using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.PiezaDetalleDTOs;

namespace AppForSEII.UT.EncargosController_test
{
    // Clase de pruebas unitarias para el método GetPiezaDetalle.
    // Hereda de AppForSEII4SqliteUT para disponer de la base de datos SQLite en memoria (_context) aislada para cada test.
    public class GetPiezaDetalle_test : AppForSEII4SqliteUT
    {
        // Constructor de la clase de test: se ejecuta automáticamente antes de cada método [Fact]
        // para poblar la base de datos en memoria con los datos iniciales de prueba (Seed Data).
        public GetPiezaDetalle_test()
        {
            // Se crean dos materiales de prueba con su ID, nombre, precio por gramo y stock disponible.
            var mat1 = new Material("PLA Blanco", 0.05m, 1000m);
            var mat2 = new Material("PETG Negro", 0.08m, 500m);

            // Se crea una pieza 3D con ID 1 y se le asocian los dos materiales anteriores como materiales válidos.
            var piezaConMateriales = new Pieza3D("Engranaje Helicoidal", 25.5m, (CategoriaPieza)0)
            {
                MaterialesValidos = new List<Material> { mat1, mat2 }
            };

            // Se añaden los materiales y la pieza al contexto de base de datos y se guardan los cambios.
            _context.AddRange( new List<Material> { mat1, mat2 } );
            _context.Add(piezaConMateriales);
            _context.SaveChanges();
        }

        // [Fact] indica un caso de prueba unitario independiente sin parámetros.
        // Prueba el escenario de éxito (200 OK): se solicita el detalle de una pieza cuyo ID sí existe en la base de datos (ID = 1).
        [Fact]
        public async Task GetPiezaDetalle_Found_test()
        {
            // Arrange: Preparación del entorno de prueba.
            // Se crea un objeto simulado (Mock) del logger necesario para instanciar EncargosController.
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

            // Se construye el DTO esperado (expectedPieza) exactamente con los mismos datos que debe devolver la consulta.
            var expectedPieza = new PiezaDetalleDTO(
                1,
                "Engranaje Helicoidal",
                25.5m,
                (CategoriaPieza)0,
                new List<MaterialParaDetalleDTO>
                {
                    new MaterialParaDetalleDTO(1, "PLA Blanco", 0.05m, 1000m),
                    new MaterialParaDetalleDTO(2, "PETG Negro", 0.08m, 500m)
                }
            );

            // Act: Ejecución del método del controlador que estamos probando pasándole el ID 1.
            var result = await controller.GetPiezaDetalle(1);

            // Assert: Verificación de los resultados.
            // 1. Se comprueba que la respuesta HTTP sea de tipo 200 OK (OkObjectResult).
            var okResult = Assert.IsType<OkObjectResult>(result);
            // 2. Se comprueba que el contenido dentro del OkObjectResult sea de tipo PiezaDetalleDTO.
            var piezaDTOActual = Assert.IsType<PiezaDetalleDTO>(okResult.Value);
            // 3. Se verifica mediante Equals (y SequenceEqual para la lista de materiales) que el DTO esperado y el recibido son idénticos.
            Assert.Equal(expectedPieza, piezaDTOActual);
        }

        // Prueba el escenario de error (404 NotFound): se solicita una pieza con un ID que no existe en la base de datos (ID = 0).
        [Fact]
        public async Task GetPiezaDetalle_NotFound_test()
        {
            // Arrange: Se simula el logger y se instancia el controlador con la base de datos en memoria.
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

            // Act: Se llama al método GetPiezaDetalle buscando el ID 0 (inexistente).
            var result = await controller.GetPiezaDetalle(0);

            // Assert: Se comprueba que el controlador devuelve una respuesta HTTP 404 (NotFoundResult).
            Assert.IsType<NotFoundResult>(result);
        }
    }
}