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
    // Clase de pruebas unitarias para el método de catálogo GetPiezasParaEncargar.
    // Hereda de AppForSEII4SqliteUT para usar la base de datos en memoria.
    public class GetPiezasParaEncargar_test : AppForSEII4SqliteUT
    {
        // Constructor que inicializa la base de datos antes de cada prueba con 3 piezas de distintas categorías y pesos.
        public GetPiezasParaEncargar_test()
        {
            var piezas = new List<Pieza3D>()
            {
                new Pieza3D("Engranaje Helicoidal", 25.5m, (CategoriaPieza)0),
                new Pieza3D("Soporte Pared", 50.0m, (CategoriaPieza)1),
                new Pieza3D("Figura articulada", 15.0m, (CategoriaPieza)2)
            };

            // Se insertan las 3 piezas en el contexto y se guardan los cambios en memoria.
            _context.AddRange(piezas);
            _context.SaveChanges();
        }

        // Prueba individual [Fact] que verifica que al no aplicar filtros (nombre = null, categoria = null)
        // el método devuelve la lista completa con las 3 piezas existentes.
        [Fact]
        public async Task GetPiezasParaEncargar_NullFilters_test()
        {
            // Arrange: Se prepara la lista esperada con los 3 DTOs correspondientes a las 3 piezas de la BD.
            List<PiezaParaEncargarDTO> expectedPiezas = new List<PiezaParaEncargarDTO>()
            {
                new PiezaParaEncargarDTO(1, "Engranaje Helicoidal", ((CategoriaPieza)0).ToString(), 25.5m),
                new PiezaParaEncargarDTO(2, "Soporte Pared", ((CategoriaPieza)1).ToString(), 50.0m),
                new PiezaParaEncargarDTO(3, "Figura articulada", ((CategoriaPieza)2).ToString(), 15.0m)
            };

            // Se crea el Mock del logger y se instancia el controlador.
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            EncargosController controller = new EncargosController(_context, logger);

            // Act: Se invoca el método pasando ambos filtros a null.
            var result = await controller.GetPiezasParaEncargar(null, null);

            // Assert: Se comprueba que devuelve 200 OK, que el valor es una lista de PiezaParaEncargarDTO
            // y que sus elementos coinciden exactamente con expectedPiezas usando el método Equals del DTO.
            var okResult = Assert.IsType<OkObjectResult>(result);
            var piezasActualResult = Assert.IsType<List<PiezaParaEncargarDTO>>(okResult.Value);
            Assert.Equal(expectedPiezas, piezasActualResult);
        }

        // [Theory] permite ejecutar este mismo método de prueba varias veces con distintos juegos de parámetros.
        // [MemberData] indica que los datos de entrada y resultados esperados provienen del método estático TestCasesFor_GetPiezasParaEncargar.
        [Theory]
        [MemberData(nameof(TestCasesFor_GetPiezasParaEncargar))]
        public async Task GetPiezasParaEncargar_Filters_test(string? filterNombre, string? filterCategoria, List<PiezaParaEncargarDTO> expectedPiezas)
        {
            // Arrange: Creación del Mock del logger e instanciación del controlador.
            var mock = new Mock<ILogger<EncargosController>>();
            ILogger<EncargosController> logger = mock.Object;
            EncargosController controller = new EncargosController(_context, logger);

            // Act: Se llama al método del controlador pasándole los filtros recibidos por parámetro en cada iteración del Theory.
            var result = await controller.GetPiezasParaEncargar(filterNombre, filterCategoria);

            // Assert: Se valida que devuelve 200 OK y que la lista filtrada devuelta es igual a la lista esperada para ese caso de prueba.
            var okResult = Assert.IsType<OkObjectResult>(result);
            var piezasActualResult = Assert.IsType<List<PiezaParaEncargarDTO>>(okResult.Value);
            Assert.Equal(expectedPiezas, piezasActualResult);
        }

        // Método estático generador de casos de prueba para el [Theory].
        // Devuelve una colección de arrays de objetos: { filtroNombre, filtroCategoria, listaEsperada }.
        public static IEnumerable<object[]> TestCasesFor_GetPiezasParaEncargar()
        {
            // Se obtienen los nombres en texto de las categorías del enumerado.
            string cat0 = ((CategoriaPieza)0).ToString();
            string cat1 = ((CategoriaPieza)1).ToString();
            string cat2 = ((CategoriaPieza)2).ToString();

            // Se instancian los DTOs individuales para armar las listas de resultados esperados.
            var dto1 = new PiezaParaEncargarDTO(1, "Engranaje Helicoidal", cat0, 25.5m);
            var dto2 = new PiezaParaEncargarDTO(2, "Soporte Pared", cat1, 50.0m);
            var dto3 = new PiezaParaEncargarDTO(3, "Figura articulada", cat2, 15.0m);

            // Caso 1: Resultado esperado al filtrar por el nombre "Engranaje" (solo devuelve dto1).
            var tc1_FilterNombre = new List<PiezaParaEncargarDTO> { dto1 };
            // Caso 2: Resultado esperado al filtrar por la categoría cat1 (solo devuelve dto2).
            var tc2_FilterCategoria = new List<PiezaParaEncargarDTO> { dto2 };

            // Lista con las combinaciones de parámetros que se inyectarán en GetPiezasParaEncargar_Filters_test.
            var allTests = new List<object[]>
            {
                new object[] { "Engranaje", null!, tc1_FilterNombre },
                new object[] { null!, cat1, tc2_FilterCategoria }
            };

            return allTests;
        }
    }
}