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
    public class PostEncargoImpresion_test : AppForSEII4SqliteUT
    {
        public PostEncargoImpresion_test()
        {
            var mat1 = new Material(1, "PLA Blanco", 0.05m, 1000m);
            var mat2 = new Material(2, "ABS Rojo", 0.06m, 500m);

            var pieza1 = new Pieza3D(1, "Engranaje Helicoidal", 25.0m, (CategoriaPieza)0)
            {
                MaterialesValidos = new List<Material> { mat1 }
            };

            _context.AddRange(new List<Material> { mat1, mat2 });
            _context.Add(pieza1);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_PostEncargoImpresion()
        {
            var encargoSinLineas = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>());

            var encargoPiezaNoExiste = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(99, 1, 2)
                });

            var encargoMaterialNoValido = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(1, 2, 2)
                });

            var encargoSinStock = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Urgente",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(1, 1, 100)
                });

            var allTests = new List<object[]>
            {
                new object[] { encargoSinLineas, "Error! El encargo debe contener al menos una pieza para imprimir." },
                new object[] { encargoPiezaNoExiste, "Error! La pieza con ID 99 no existe." },
                new object[] { encargoMaterialNoValido, "Error! El material con ID 2 no es válido para la pieza 'Engranaje Helicoidal'." },
                new object[] { encargoSinStock, "Error! No hay suficiente stock del material 'PLA Blanco' para imprimir 100 unidades de 'Engranaje Helicoidal'." }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_PostEncargoImpresion))]
        public async Task PostEncargoImpresion_Error_test(EncargoParaCrearDTO encargoDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

            // Act
            var result = await controller.PostEncargoImpresion(encargoDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorEsperado, errorActual);
        }

        [Fact]
        public async Task PostEncargoImpresion_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

            var lineas = new List<LineaEncargoParaCrearDTO>
            {
                new LineaEncargoParaCrearDTO(1, 1, 2)
            };

            var encargoDTO = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Impresión estándar",
                (MetodoPago)0, lineas);

            var expectedEncargoDTO = new EncargoParaCrearDTO(
                "Juan", "Pérez", "Calle Mayor 1", "600123456", "Impresión estándar",
                (MetodoPago)0, new List<LineaEncargoParaCrearDTO>
                {
                    new LineaEncargoParaCrearDTO(1, 1, 2)
                });

            // Act
            var result = await controller.PostEncargoImpresion(encargoDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var actualEncargoDTO = Assert.IsType<EncargoParaCrearDTO>(createdResult.Value);

            Assert.Equal(expectedEncargoDTO, actualEncargoDTO);
        }
    }
}