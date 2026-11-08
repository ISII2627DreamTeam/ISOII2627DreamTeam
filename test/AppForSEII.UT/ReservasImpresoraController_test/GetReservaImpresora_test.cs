using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;

namespace AppForSEII.UT.ReservasImpresoraController_test
{
    public class GetReservaImpresora_test : AppForSEII4SqliteUT
    {
        public GetReservaImpresora_test()
        {
            var impresora = new Impresora3D("Ender 3", TipoImpresora.Filamento, "Creality Ender 3", "Impresora de filamento", 0.10m, 2.50m) { Id = 1 };
            var reserva = new ReservaImpresora(DateTime.Now, "Juan", "Perez", "Calle Falsa 123", 5.00m, MetodoPago.TarjetaCredito) { Id = 1 };

            var linea = new LineaReserva
            {
                Id = 1,
                TiempoReserva = TiempoReserva.DosHoras,
                PrecioSubtotal = 5.00m,
                Impresora3DId = impresora.Id,
                Impresora = impresora,
                ReservaImpresora = reserva
            };
            reserva.LineasReserva.Add(linea);

            _context.ReservaImpresoras.Add(reserva);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetReservaImpresora_ReturnsOk_WhenIdExists()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReservasImpresoraController>>();
            var controller = new ReservasImpresoraController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetReservaImpresora(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ReservaImpresoraDTO>(okResult.Value);
            Assert.Equal(1, dto.Id);
            Assert.Equal("Juan", dto.NombreCliente);
            Assert.Equal(5.00m, dto.PrecioTotal);
            var linea = Assert.Single(dto.Lineas);
            Assert.Equal(1, linea.Impresora3DId);
            Assert.Equal(TiempoReserva.DosHoras, linea.TiempoReserva);
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