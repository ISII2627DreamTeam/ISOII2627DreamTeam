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

        public static IEnumerable<object?[]> TestCasesFor_GetPiezasParaEncargar()
        {
            string cat0 = ((CategoriaPieza)0).ToString();
            string cat1 = ((CategoriaPieza)1).ToString();
            string cat2 = ((CategoriaPieza)2).ToString();

            var dto1 = new PiezaParaEncargarDTO(1, "Engranaje Helicoidal", cat0, 25.5m);
            var dto2 = new PiezaParaEncargarDTO(2, "Soporte Pared", cat1, 50.0m);
            var dto3 = new PiezaParaEncargarDTO(3, "Figura articulada", cat2, 15.0m);

            var tc1_WithoutFilters = new List<PiezaParaEncargarDTO> { dto1, dto2, dto3 };
            var tc2_FilterNombre = new List<PiezaParaEncargarDTO> { dto1 };
            var tc3_FilterCategoria = new List<PiezaParaEncargarDTO> { dto2 };
            var tc4_BothFilters = new List<PiezaParaEncargarDTO> { dto3 };

            var allTests = new List<object?[]>
            {
                new object?[] { null, null, tc1_WithoutFilters },
                new object?[] { "Engranaje", null, tc2_FilterNombre },
                new object?[] { null, cat1, tc3_FilterCategoria },
                new object?[] { "Figura", cat2, tc4_BothFilters }
            };

            return allTests;
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
    }
}