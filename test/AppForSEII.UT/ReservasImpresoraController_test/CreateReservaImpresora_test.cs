using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.UT.ReservasImpresoraController_test
{
    public class CreateReservaImpresora_test : AppForSEII4SqliteUT
    {
        private readonly List<Impresora3D> _impresoras;
        private static readonly DateTime Fecha = new DateTime(2026, 11, 8, 10, 0, 0);

        public CreateReservaImpresora_test()
        {
            _impresoras = new List<Impresora3D>
            {
                new Impresora3D("Ender 3", TipoImpresora.Filamento, "Creality Ender 3", "Impresora de filamento", 0.10m, 2.00m),
                new Impresora3D("Photon Mono", TipoImpresora.Resina, "Anycubic Photon", "Impresora de resina", 0.15m, 4.00m)
            };

            _context.AddRange(_impresoras);
            _context.SaveChanges();
        }

        private ReservasImpresoraController CrearController()
        {
            var mockLogger = new Mock<ILogger<ReservasImpresoraController>>();
            return new ReservasImpresoraController(_context, mockLogger.Object);
        }

        private static ReservaImpresoraDTO CrearReservaDto()
        {
            var reserva = new ReservaImpresora(Fecha, "Jose Miguel", "Martinez", "Avenida Avenidosa 456", 0m, MetodoPago.PayPal);
            return new ReservaImpresoraDTO(reserva);
        }

        private static LineaReservaDTO CrearLineaDto(int impresoraId, TiempoReserva tiempo)
        {
            return new LineaReservaDTO(new LineaReserva(1, tiempo, 0m, impresoraId, -1));
        }

        [Theory]
        [InlineData(TiempoReserva.UnaHora, 1)]
        [InlineData(TiempoReserva.DosHoras, 2)]
        [InlineData(TiempoReserva.TresHoras, 3)]
        [InlineData(TiempoReserva.CuatroHoras, 4)]
        [InlineData(TiempoReserva.CincoHoras, 5)]
        [InlineData(TiempoReserva.UnDia, 24)]
        [InlineData(TiempoReserva.DosDias, 48)]
        public async Task CreateReservaImpresora_ReturnsCreated_AndCalculatesSubtotal(TiempoReserva tiempo, int horas)
        {
            // Arrange
            var controller = CrearController();
            var dto = CrearReservaDto();
            dto.Lineas.Add(CrearLineaDto(_impresoras[0].Id, tiempo));
            decimal subtotal = 2.00m * horas;

            // Act
            var result = await controller.CreateReservaImpresora(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<ReservaImpresoraDTO>(createdResult.Value);
            var expectedReserva = new ReservaImpresora(actual.FechaReserva, "Jose Miguel", "Martinez", "Avenida Avenidosa 456", subtotal, MetodoPago.PayPal);
            expectedReserva.Id = actual.Id;
            var expectedLinea = new LineaReserva(1, tiempo, subtotal, _impresoras[0].Id, actual.Id);
            expectedLinea.Impresora = _impresoras[0];
            expectedReserva.LineasReserva.Add(expectedLinea);
            Assert.Equal(new ReservaImpresoraDTO(expectedReserva), actual);
        }

        [Fact]
        public async Task CreateReservaImpresora_ReturnsCreated_WithSeveralLines_SumsTotal()
        {
            // Arrange
            var controller = CrearController();
            var dto = CrearReservaDto();
            dto.Lineas.Add(CrearLineaDto(_impresoras[0].Id, TiempoReserva.TresHoras));
            dto.Lineas.Add(CrearLineaDto(_impresoras[1].Id, TiempoReserva.UnDia));

            // Act
            var result = await controller.CreateReservaImpresora(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<ReservaImpresoraDTO>(createdResult.Value);
            var expectedReserva = new ReservaImpresora(actual.FechaReserva, "Jose Miguel", "Martinez", "Avenida Avenidosa 456", 102.00m, MetodoPago.PayPal);
            expectedReserva.Id = actual.Id;
            var expectedLinea1 = new LineaReserva(1, TiempoReserva.TresHoras, 6.00m, _impresoras[0].Id, actual.Id);
            expectedLinea1.Impresora = _impresoras[0];
            var expectedLinea2 = new LineaReserva(2, TiempoReserva.UnDia, 96.00m, _impresoras[1].Id, actual.Id);
            expectedLinea2.Impresora = _impresoras[1];
            expectedReserva.LineasReserva.Add(expectedLinea1);
            expectedReserva.LineasReserva.Add(expectedLinea2);
            Assert.Equal(new ReservaImpresoraDTO(expectedReserva), actual);
        }

        [Fact]
        public async Task CreateReservaImpresora_ReturnsBadRequest_WhenLineasEsNull()
        {
            // Arrange
            var controller = CrearController();
            var dto = CrearReservaDto();
            dto.Lineas = null!;

            // Act
            var result = await controller.CreateReservaImpresora(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(0, await _context.ReservaImpresoras.CountAsync());
        }

        [Fact]
        public async Task CreateReservaImpresora_ReturnsBadRequest_WhenLineasVacias()
        {
            // Arrange
            var controller = CrearController();
            var dto = CrearReservaDto();

            // Act
            var result = await controller.CreateReservaImpresora(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(0, await _context.ReservaImpresoras.CountAsync());
        }

        [Fact]
        public async Task CreateReservaImpresora_ReturnsBadRequest_WhenTiempoReservaNoValido()
        {
            // Arrange
            var controller = CrearController();
            var dto = CrearReservaDto();
            dto.Lineas.Add(CrearLineaDto(_impresoras[0].Id, (TiempoReserva)99));

            // Act
            var result = await controller.CreateReservaImpresora(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(0, await _context.ReservaImpresoras.CountAsync());
        }

        [Fact]
        public async Task CreateReservaImpresora_ReturnsBadRequest_WhenImpresoraNoExiste()
        {
            // Arrange
            var controller = CrearController();
            var dto = CrearReservaDto();
            dto.Lineas.Add(CrearLineaDto(999, TiempoReserva.UnaHora));

            // Act
            var result = await controller.CreateReservaImpresora(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(0, await _context.ReservaImpresoras.CountAsync());
        }
    }
}