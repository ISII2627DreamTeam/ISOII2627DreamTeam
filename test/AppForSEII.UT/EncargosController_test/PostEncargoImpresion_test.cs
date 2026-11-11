using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.EncargoParaCrearDTOs;

namespace AppForSEII.UT.EncargosController_test
{
    // Clase de pruebas unitarias para el método POST (PostEncargoImpresion).
    public class PostEncargoImpresion_test : AppForSEII4SqliteUT
    {
        // Constructor que prepara los datos en la BD en memoria antes de cada ejecución:
        // - Material 1: válido para la Pieza 1 y con 1000g de stock.
        // - Material 2: NO asociado a la Pieza 1 (sirve para probar el fallo de material no compatible).
        // - Pieza 1: pesa 25g y solo tiene como material válido el Material 1.
        public PostEncargoImpresion_test()
        {
            var mat1 = new Material("PLA Blanco", 0.05m, 1000m);
            var mat2 = new Material("ABS Rojo", 0.06m, 500m);

            var pieza1 = new Pieza3D("Engranaje Helicoidal", 25.0m, (CategoriaPieza)0);
            pieza1.MaterialesValidos.Add(mat1);

            _context.AddRange(new List<Material> { mat1, mat2 });
            _context.Add(pieza1);
            _context.SaveChanges();
        }

        // Método estático que suministra los 4 casos de prueba de error (400 BadRequest) al método [Theory].
        // Cada caso empareja un DTO de entrada inválido con el mensaje de error exacto que debe devolver el controlador.
        public static IEnumerable<object[]> TestCasesFor_PostEncargoImpresion()
        {
            // Caso de error 1: Encargo con la lista de líneas vacía (sin piezas solicitadas).
            var encargoSinLineas = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>());

            // Caso de error 2: Se solicita una pieza con ID 99 que no existe en la base de datos.
            var encargoPiezaNoExiste = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(99, 1, 2)
                });

            // Caso de error 3: Se solicita la Pieza 1 con el Material 2, el cual no está en su lista de MaterialesValidos.
            var encargoMaterialNoValido = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(1, 2, 2)
                });

            // Caso de error 4: Se piden 100 unidades de la Pieza 1 (100 * 25g = 2500g), superando los 1000g de stock del Material 1.
            var encargoSinStock = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(1, 1, 100)
                });

            // Se agrupan los 4 escenarios junto con sus respectivos mensajes de validación esperados.
            var allTests = new List<object[]>
            {
                new object[] { encargoSinLineas, "Error! El encargo debe contener al menos una pieza para imprimir." },
                new object[] { encargoPiezaNoExiste, "Error! La pieza con ID 99 no existe." },
                new object[] { encargoMaterialNoValido, "Error! El material con ID 2 no es válido para la pieza 'Engranaje Helicoidal'." },
                new object[] { encargoSinStock, "Error! No hay suficiente stock del material 'PLA Blanco' para imprimir 100 unidades de 'Engranaje Helicoidal'." }
            };

            return allTests;
        }

        // Prueba parametrizada [Theory] que ejecuta los 4 escenarios de error definidos arriba
        // y verifica que el controlador responde con 400 BadRequest y el mensaje de validación correspondiente.
        [Theory]
        [MemberData(nameof(TestCasesFor_PostEncargoImpresion))]
        public async Task PostEncargoImpresion_Error_test(EncargoParaCrearDTO encargoDTO, string errorEsperado)
        {
            // Arrange: Se inicializa el Mock del logger y el controlador.
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

            // Act: Se llama a PostEncargoImpresion con el DTO erróneo de la iteración actual.
            var result = await controller.PostEncargoImpresion(encargoDTO);

            // Assert:
            // 1. Se verifica que la respuesta sea un HTTP 400 (BadRequestObjectResult).
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            // 2. Se extrae el objeto ValidationProblemDetails que contiene los errores del ModelState.
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            // 3. Se obtiene el texto del primer mensaje de error devuelto.
            var errorActual = problemDetails.Errors.First().Value[0];

            // 4. Se comprueba que el mensaje de error devuelto comience con el texto esperado.
            Assert.StartsWith(errorEsperado, errorActual);
        }

        // Prueba unitaria [Fact] para el caso de éxito (201 Created):
        // todos los datos son válidos, hay stock suficiente y el encargo se guarda correctamente en la base de datos.
        [Fact]
        public async Task PostEncargoImpresion_Success_test()
        {
            // Arrange: Se prepara el logger simulado y el controlador.
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

            // Se crea una línea válida: Pieza 1, Material 1 (compatible) y Cantidad 2 (2 * 25g = 50g <= 1000g de stock).
            var lineas = new List<LineaEncargoParaCrearDTO>
            {
                new LineaEncargoParaCrearDTO(1, 1, 2)
            };

            // DTO de entrada que se enviará al método POST.
            var encargoDTO = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Impresión estándar",
                (MetodoPago)0, lineas);

            // DTO esperado con el que se comparará la respuesta devuelta por el controlador.
            var expectedEncargoDTO = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Impresión estándar",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(1, 1, 2)
                });

            // Act: Se ejecuta la creación del encargo.
            var result = await controller.PostEncargoImpresion(encargoDTO);

            // Assert:
            // 1. Se comprueba que el resultado sea HTTP 201 Created (CreatedAtActionResult).
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            // 2. Se extrae el DTO devuelto dentro del cuerpo de la respuesta.
            var actualEncargoDTO = Assert.IsType<EncargoParaCrearDTO>(createdResult.Value);

            // 3. Se comprueba mediante el método Equals (y SequenceEqual en las líneas) que ambos DTOs son iguales.
            Assert.Equal(expectedEncargoDTO, actualEncargoDTO);
        }
    }
}