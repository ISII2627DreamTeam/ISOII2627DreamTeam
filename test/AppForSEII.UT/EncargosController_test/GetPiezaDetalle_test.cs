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
    public class GetPiezaDetalle_test : AppForSEII4SqliteUT
    {
        public GetPiezaDetalle_test()
        {
            var mat1 = new Material(1, "PLA Blanco", 0.05m, 1000m);
            var mat2 = new Material(2, "PETG Negro", 0.08m, 500m);

            var piezaConMateriales = new Pieza3D(1, "Engranaje Helicoidal", 25.5m, (CategoriaPieza)0);
            piezaConMateriales.MaterialesValidos.Add(mat1);
            piezaConMateriales.MaterialesValidos.Add(mat2);

            _context.AddRange(new List<Material> { mat1, mat2 });
            _context.Add(piezaConMateriales);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetPiezaDetalle_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

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

            // Act
            var result = await controller.GetPiezaDetalle(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var piezaDTOActual = Assert.IsType<PiezaDetalleDTO>(okResult.Value);
            Assert.Equal(expectedPieza, piezaDTOActual);
        }

        [Fact]
        public async Task GetPiezaDetalle_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            var controller = new EncargosController(_context, logger);

            // Act
            var result = await controller.GetPiezaDetalle(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}