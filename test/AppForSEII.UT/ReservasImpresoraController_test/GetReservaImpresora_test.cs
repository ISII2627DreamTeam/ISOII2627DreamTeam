using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;

namespace AppForSEII.UT.ReservasImpresoraController_test
{
    public class GetReservaImpresora_test : AppForSEII4SqliteUT
    {
        private readonly ReservaImpresora _reserva;

        public GetReservaImpresora_test()
        {
            var impresora = new Impresora3D("Ender 3", TipoImpresora.Filamento, "Creality Ender 3", "Impresora de filamento", 0.10m, 2.50m) { Id = 1 };
            _reserva = new ReservaImpresora(new DateTime(2026, 11, 8, 10, 0, 0), "Juan", "Perez", "Calle Falsa 123", 5.00m, MetodoPago.TarjetaCredito) { Id = 1 };
            var linea = new LineaReserva(1, TiempoReserva.DosHoras, 5.00m, 1, 1)
            {
                Impresora = impresora,
                ReservaImpresora = _reserva
            };
            _reserva.LineasReserva.Add(linea);

            _context.ReservaImpresoras.Add(_reserva);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetReservaImpresora_ReturnsOk_WhenIdExists()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReservasImpresoraController>>();
            var controller = new ReservasImpresoraController(_context, mockLogger.Object);
            var expected = new ReservaImpresoraDTO(_reserva);

            // Act
            var result = await controller.GetReservaImpresora(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<ReservaImpresoraDTO>(okResult.Value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetReservaImpresora_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReservasImpresoraController>>();
            var controller = new ReservasImpresoraController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetReservaImpresora(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}