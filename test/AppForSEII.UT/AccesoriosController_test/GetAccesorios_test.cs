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

// Espacio de nombres que agrupa todos los tests unitarios relacionados con AccesoriosController.
namespace AppForSEII.UT.AccesoriosController_test
{
    // Declaración de la clase de pruebas. Hereda de AppForSEII4SqliteUT para instanciar automáticamente la base de datos SQLite en memoria (_context).
    public class GetAccesorios_test : AppForSEII4SqliteUT
    {
        // Constructor de la clase de prueba. Actúa como el ARRANGE GLOBAL que se ejecuta antes de cada evaluación para preparar la base de datos.
        public GetAccesorios_test()
        {
            // Instanciamos una lista temporal con tres entidades Accesorio simulando el stock inicial de la tienda.
            var accesorios = new List<Accesorio>
            {
                // Se utilizan los constructores parametrizados del modelo para instanciar los objetos evitando inicializadores con llaves {}.
                new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m),
                new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m),
                new Accesorio(3, "Alicates", CategoriaAccesorio.Herramientas, "Universal", 30, 8.00m)
            };

            // Añadimos la lista completa de accesorios al contexto de Entity Framework (en memoria).
            _context.AddRange(accesorios);
            // Guardamos los cambios para que se inserten los registros simulados en las tablas de SQLite.
            _context.SaveChanges();
        }

        // Método estático que actúa como generador de casos de prueba (MemberData) para el test tipo Theory.
        public static IEnumerable<object?[]> TestCasesFor_GetAccesorios()
        {
            // Creamos una lista de DTOs esperados instanciando los modelos y pasándolos por el constructor de mapeo del DTO.
            List<AccesorioDTO> accesorioDTOs = new List<AccesorioDTO>()
            {
                // Replicamos exactamente los mismos datos del Arrange global para tener el resultado esperado convertido en DTO.
                new AccesorioDTO( new Accesorio(1, "Boquilla 0.4mm", CategoriaAccesorio.Boquillas, "Ender 3", 50, 5.99m)),
                new AccesorioDTO( new Accesorio(2, "Cama de Cristal", CategoriaAccesorio.BasesDeImpresion, "Ender 3", 20, 15.50m)),
                new AccesorioDTO( new Accesorio(3, "Alicates", CategoriaAccesorio.Herramientas, "Universal", 30, 8.00m))
            };

            // Caso 1: Si no enviamos filtros, esperamos recibir la lista completa (los 3 DTOs).
            var TC1WithOutFilters = new List<AccesorioDTO> { accesorioDTOs[0], accesorioDTOs[1], accesorioDTOs[2] };
            // Caso 2: Si filtramos por el nombre "Boquilla", esperamos recibir únicamente el primer DTO.
            var TC2NombreFilter = new List<AccesorioDTO> { accesorioDTOs[0] };
            // Caso 3: Si filtramos por categoría "BasesDeImpresion", esperamos recibir el segundo DTO.
            var TC3CategoriaFilter = new List<AccesorioDTO> { accesorioDTOs[1] };
            // Caso 4: Si enviamos ambos filtros ("Alicates" y "Herramientas"), esperamos el tercer DTO.
            var TC4BothFilters = new List<AccesorioDTO> { accesorioDTOs[2] };

            // Empaquetamos todos los casos de prueba en una lista de arrays de objetos genéricos.
            var allTests = new List<object?[]> {
                // Formato de cada array: { Parámetro 'nombre', Parámetro 'categoria', Lista Resultante Esperada }
                new object?[] { null, null, TC1WithOutFilters },
                new object?[] { "Boquilla", null, TC2NombreFilter },
                new object?[] { null, "BasesDeImpresion", TC3CategoriaFilter },
                new object?[] { "Alicates", "Herramientas", TC4BothFilters }
            };

            // Retornamos la colección completa para que el atributo MemberData la procese iteración por iteración.
            return allTests;
        }

        // Atributo Theory de XUnit que indica que este test se ejecutará múltiples veces con distintos datos de entrada.
        [Theory]
        // Vincula el método generador de datos (TestCasesFor_GetAccesorios) con los parámetros de entrada del test.
        [MemberData(nameof(TestCasesFor_GetAccesorios))]
        // La firma del método recibe dinámicamente el nombre, la categoría y la lista de DTOs esperada en cada iteración del Theory.
        public async Task GetAccesorios_DataDriven_test(string? nombre, string? categoria, List<AccesorioDTO> expected)
        {
            // ARRANGE (Preparación): Instanciamos un Mock vacío del ILogger porque no necesitamos registrar logs reales en consola durante el test.
            var mock = new Mock<ILogger<AccesoriosController>>();
            // Extraemos el objeto falso generado por Moq para pasarlo como dependencia.
            ILogger<AccesoriosController> logger = mock.Object;
            // Instanciamos manualmente el controlador pasando la base de datos de pruebas heredada (_context) y el logger simulado.
            AccesoriosController controller = new AccesoriosController(_context, logger);

            // ACT (Acción): Ejecutamos el método del controlador pasándole los filtros dinámicos recibidos por parámetro del MemberData.
            var result = await controller.GetAccesorios(nombre, categoria);

            // ASSERT (Verificación): Confirmamos que el método devolvió un código HTTP 200 OK (OkObjectResult).
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Extraemos el valor del resultado HTTP y confirmamos que su tipo es exactamente una Lista de AccesorioDTO.
            var actual = Assert.IsType<List<AccesorioDTO>>(okResult.Value);
            // Comparamos el contenido de la lista esperada con la lista obtenida utilizando el método Equals sobrescrito en AccesorioDTO.
            Assert.Equal(expected, actual);
        }
    }
}