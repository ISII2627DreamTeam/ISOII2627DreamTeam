using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.EncargoImpresionDTOs;

namespace AppForSEII.UT.EncargosController_test
{
    public class GetPiezasParaEncargar_test : AppForSEII4SqliteUT
    {
        public GetPiezasParaEncargar_test()
        {
            var piezas = new List<Pieza3D>()
            {
                new Pieza3D(1, "Engranaje Helicoidal", 25.5m, (CategoriaPieza)0),
                new Pieza3D(2, "Soporte Pared", 50.0m, (CategoriaPieza)1),
                new Pieza3D(3, "Figura articulada", 15.0m, (CategoriaPieza)2)
            };

            _context.AddRange(piezas);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetPiezasParaEncargar_NullFilters_test()
        {
            // Arrange
            List<PiezaParaEncargarDTO> expectedPiezas = new List<PiezaParaEncargarDTO>()
            {
                new PiezaParaEncargarDTO(1, "Engranaje Helicoidal", ((CategoriaPieza)0).ToString(), 25.5m),
                new PiezaParaEncargarDTO(2, "Soporte Pared", ((CategoriaPieza)1).ToString(), 50.0m),
                new PiezaParaEncargarDTO(3, "Figura articulada", ((CategoriaPieza)2).ToString(), 15.0m)
            };

            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            EncargosController controller = new EncargosController(_context, logger);

            // Act
            var result = await controller.GetPiezasParaEncargar(null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var piezasActualResult = Assert.IsType<List<PiezaParaEncargarDTO>>(okResult.Value);
            Assert.Equal(expectedPiezas, piezasActualResult);
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetPiezasParaEncargar))]
        public async Task GetPiezasParaEncargar_Filters_test(string? filterNombre, string? filterCategoria, List<PiezaParaEncargarDTO> expectedPiezas)
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            EncargosController controller = new EncargosController(_context, logger);

            // Act
            var result = await controller.GetPiezasParaEncargar(filterNombre, filterCategoria);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var piezasActualResult = Assert.IsType<List<PiezaParaEncargarDTO>>(okResult.Value);
            Assert.Equal(expectedPiezas, piezasActualResult);
        }

        public static IEnumerable<object[]> TestCasesFor_GetPiezasParaEncargar()
        {
            string cat0 = ((CategoriaPieza)0).ToString();
            string cat1 = ((CategoriaPieza)1).ToString();
            string cat2 = ((CategoriaPieza)2).ToString();

            var dto1 = new PiezaParaEncargarDTO(1, "Engranaje Helicoidal", cat0, 25.5m);
            var dto2 = new PiezaParaEncargarDTO(2, "Soporte Pared", cat1, 50.0m);
            var dto3 = new PiezaParaEncargarDTO(3, "Figura articulada", cat2, 15.0m);

            var tc1_FilterNombre = new List<PiezaParaEncargarDTO> { dto1 };
            var tc2_FilterCategoria = new List<PiezaParaEncargarDTO> { dto2 };

            var allTests = new List<object[]>
            {
                new object[] { "Engranaje", null!, tc1_FilterNombre },
                new object[] { null!, cat1, tc2_FilterCategoria }
            };

            return allTests;
        }
    }
}