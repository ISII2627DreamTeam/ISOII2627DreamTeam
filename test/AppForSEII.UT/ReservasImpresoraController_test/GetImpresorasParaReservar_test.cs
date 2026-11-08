using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;

namespace AppForSEII.UT.ReservasImpresoraController_test
{
    public class GetImpresorasParaReservar_test : AppForSEII4SqliteUT
    {
        public GetImpresorasParaReservar_test()
        {
            var impresoras = new List<Impresora3D>
            {
                new Impresora3D("Ender 3", TipoImpresora.Filamento, "Creality Ender 3", "Impresora de filamento", 0.10m, 2.50m) { Id = 1 },
                new Impresora3D("Photon Mono", TipoImpresora.Resina, "Anycubic Photon", "Impresora de resina", 0.15m, 4.00m) { Id = 2 },
                new Impresora3D("Form 3", TipoImpresora.Polimero, "Formlabs Form 3", "Impresora de polimero", 0.20m, 6.00m) { Id = 3 }
            };

            _context.AddRange(impresoras);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetImpresorasParaReservar()
        {
            var dto1 = new Impresora3DDTO(new Impresora3D("Ender 3", TipoImpresora.Filamento, "Creality Ender 3", "Impresora de filamento", 0.10m, 2.50m) { Id = 1 });
            var dto2 = new Impresora3DDTO(new Impresora3D("Photon Mono", TipoImpresora.Resina, "Anycubic Photon", "Impresora de resina", 0.15m, 4.00m) { Id = 2 });
            var dto3 = new Impresora3DDTO(new Impresora3D("Form 3", TipoImpresora.Polimero, "Formlabs Form 3", "Impresora de polimero", 0.20m, 6.00m) { Id = 3 });

            var TC1SinFiltros = new List<Impresora3DDTO> { dto1, dto2, dto3 };
            var TC2FiltroNombre = new List<Impresora3DDTO> { dto1 };
            var TC3FiltroTipo = new List<Impresora3DDTO> { dto2 };
            var TC4AmbosFiltros = new List<Impresora3DDTO> { dto3 };

            return new List<object?[]>
            {
                new object?[] { null, null, TC1SinFiltros },
                new object?[] { "Ender", null, TC2FiltroNombre },
                new object?[] { null, "Resina", TC3FiltroTipo },
                new object?[] { "Form", "Polimero", TC4AmbosFiltros }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetImpresorasParaReservar))]
        public async Task GetImpresorasParaReservar_DataDriven_test(string? nombre, string? tipo, List<Impresora3DDTO> expected)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReservasImpresoraController>>();
            var controller = new ReservasImpresoraController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetImpresorasParaReservar(nombre, tipo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<Impresora3DDTO>>(okResult.Value);
            Assert.Equal(expected, actual);
        }
    }
}