using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.CompraAccesoriosDTOs;
using SQLitePCL;

namespace AppForSEII.UT.AccesoriosController_test
{
    public class GetAccesorios_test : AppForSEII4SqliteUT
    {
        public GetAccesorios_test()
        {
            var accesorios = new List<Accesorio>
            {
                new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m),
                new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m),
                new Accesorio(3, "Alicates", CategoriaAccesorio.Herramientas, "Universal", 30, 8.00m)
            };

            _context.AddRange(accesorios);
            _context.SaveChanges();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetAccesorios()
        {

            List<AccesorioDTO> accesorioDTOs = new List<AccesorioDTO>()
            {
                new AccesorioDTO( new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m)),
                new AccesorioDTO( new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m)),
                new AccesorioDTO( new Accesorio(3, "Alicates", CategoriaAccesorio.Herramientas, "Universal", 30, 8.00m))
            };

            var TC1WithOutFilters = new List<AccesorioDTO> { accesorioDTOs[0], accesorioDTOs[1], accesorioDTOs[2] };
            var TC2NombreFilter = new List<AccesorioDTO> { accesorioDTOs[0] };
            var TC3CategoriaFilter = new List<AccesorioDTO> { accesorioDTOs[1] };
            var TC4BothFilters = new List<AccesorioDTO> { accesorioDTOs[2] };

            var allTests = new List<object?[]> {
                new object?[] { null, null, TC1WithOutFilters },
                new object?[] { "Boquilla", null, TC2NombreFilter },
                new object?[] { null, "BasesImpresion", TC3CategoriaFilter },
                new object?[] { "Alicates", "Herramientas", TC4BothFilters }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetAccesorios))]
        public async Task GetAccesorios_DataDriven_test(string? nombre, string? categoria, List<AccesorioDTO> expected)
        {
            var mock = new Mock<ILogger<AccesoriosController>>();
            ILogger<AccesoriosController> logger = mock.Object;
            AccesoriosController controller = new AccesoriosController(_context, logger);

            var result = await controller.GetAccesorios(nombre, categoria);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<AccesorioDTO>>(okResult.Value);
            Assert.Equal(expected, actual);
        }
    }
}