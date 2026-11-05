using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq; 
using Xunit; 
using AppForSEII.API.Controllers;
using AppForSEII.API.Models;
using AppForSEII.API.DTOs.CompraModelos3DDTOs;

namespace AppForSEII.UT.Modelos3DController_test
{
    // Heredo de AppForSEII4SqliteUT para usar la base de datos SQLite.
    // Así los tests están aislados y no tocan ni rompen la base de datos real.
    public class GetModelos3D_test : AppForSEII4SqliteUT 
    {
        // CONSTRUCTOR: Como la BD de pruebas nace vacía en cada ejecución, 
        // inyecto aquí a mano mis 3 modelos de prueba para que el controlador tenga algo que buscar.
        public GetModelos3D_test()
        {
            var modelos = new List<Modelo3D>() {
                new Modelo3D { Id = 1, Nombre = "Modelo 3D A", Categoría = "Juguetes", Formato = FormatoModelo3D.STL, Precio = 5.99m },
                new Modelo3D { Id = 2, Nombre = "Modelo 3D B", Categoría = "Oficina", Formato = FormatoModelo3D.OBJ, Precio = 2.50m },
                new Modelo3D { Id = 3, Nombre = "Modelo 3D C", Categoría = "Mecánica", Formato = FormatoModelo3D.TresMF, Precio = 12.00m }
            };
            
            _context.AddRange(modelos);
            _context.SaveChanges();
        }

        // [Fact] le dice a xUnit que este es un test estático, de una sola pasada y sin parámetros.
        [Fact]
        public async Task GetModelos3D_NullFilters_test()
        {
            // ARRANGE (Organizar): Preparo el escenario. 
            // Defino la lista exacta de DTOs que ESPERO que me devuelva el controlador si le pido todo el catálogo.
            List<Modelo3DDTO> expectedModelos = new List<Modelo3DDTO>() {
                new Modelo3DDTO(new Modelo3D { Id = 1, Nombre = "Modelo 3D A", Categoría = "Juguetes", Formato = FormatoModelo3D.STL, Precio = 5.99m }),
                new Modelo3DDTO(new Modelo3D { Id = 2, Nombre = "Modelo 3D B", Categoría = "Oficina", Formato = FormatoModelo3D.OBJ, Precio = 2.50m }),
                new Modelo3DDTO(new Modelo3D { Id = 3, Nombre = "Modelo 3D C", Categoría = "Mecánica", Formato = FormatoModelo3D.TresMF, Precio = 12.00m })
            };

            // Uso Moq para crear un Logger de mentira (falso). 
            // Esto evita que el controlador explote al pedir su dependencia de logs en el constructor.
            var mock = new Mock<ILogger<Modelos3DController>>();
            ILogger<Modelos3DController> logger = mock.Object;
            Modelos3DController controller = new Modelos3DController(_context, logger);

            // ACT (Actuar): Ejecuto la acción real.
            // Llamo al método pasándole null en ambos filtros, simulando un cliente que quiere ver todo.
            var result = await controller.GetModelos3D(null, null);

            // ASSERT (Afirmar): Compruebo que los resultados coinciden con lo que yo esperaba.
            // 1. Compruebo que el controlador devuelve un estado HTTP 200 OK.
            var okResult = Assert.IsType<OkObjectResult>(result);
            // 2. Compruebo que el contenido (Payload) que viene dentro del OK es una lista de mis DTOs.
            var modelosActualResult = Assert.IsType<List<Modelo3DDTO>>(okResult.Value);
            // 3. Verifico que la lista esperada y la real son idénticas. 
            Assert.Equal(expectedModelos, modelosActualResult); 
        }

        // [Theory] le dice a xUnit que este es un test dinámico (Data-driven). Se ejecutará varias veces con distintos parámetros.
        [Theory]
        // [MemberData] enlaza este test con el método de abajo, indicándole de dónde sacar los parámetros a inyectar.
        [MemberData(nameof(TestCasesFor_GetModelos3D))] 
        public async Task GetModelos3D_Filters_test(string? filterNombre, string? filterCategoria, List<Modelo3DDTO> expectedModelos)
        {
            // ARRANGE: Instancio el controlador con el logger falso igual que arriba.
            var mock = new Mock<ILogger<Modelos3DController>>();
            ILogger<Modelos3DController> logger = mock.Object;
            Modelos3DController controller = new Modelos3DController(_context, logger);

            // ACT: Llamo al método de la API, pero esta vez inyectando los filtros variables que entran por parámetro.
            var result = await controller.GetModelos3D(filterNombre, filterCategoria);

            // ASSERT: Comprueba que el resultado coincida con lo esperado para cada filtro.
            var okResult = Assert.IsType<OkObjectResult>(result);
            var modelosActualResult = Assert.IsType<List<Modelo3DDTO>>(okResult.Value);
            Assert.Equal(expectedModelos, modelosActualResult);
        }

        // Método estático que funciona casos de prueba. Devuelve las combinaciones que usará el [Theory].
        public static IEnumerable<object[]> TestCasesFor_GetModelos3D()
        {
            // Preparo los 3 DTOs sueltos para combinarlos más fácilmente.
            var dto1 = new Modelo3DDTO(new Modelo3D { Id = 1, Nombre = "Modelo 3D A", Categoría = "Juguetes", Formato = FormatoModelo3D.STL, Precio = 5.99m });
            var dto2 = new Modelo3DDTO(new Modelo3D { Id = 2, Nombre = "Modelo 3D B", Categoría = "Oficina", Formato = FormatoModelo3D.OBJ, Precio = 2.50m });
            var dto3 = new Modelo3DDTO(new Modelo3D { Id = 3, Nombre = "Modelo 3D C", Categoría = "Mecánica", Formato = FormatoModelo3D.TresMF, Precio = 12.00m });

            // Tc1: Si el cliente filtra por nombre "Modelo 3D A", ESPERO que la API devuelva solo el dto1.
            var tc1_FilterNombre = new List<Modelo3DDTO> { dto1 }; 
            // Tc2: Si el cliente filtra por categoría "Oficina", ESPERO que la API devuelva solo el dto2.
            var tc2_FilterCategoria = new List<Modelo3DDTO> { dto2 }; 

            // Construyo la lista de escenarios. Cada object[] es una ejecución completa del test.
            var allTests = new List<object[]> {
                // caso 1:filterNombre = "Modelo 3D A", filterCategoria = null, output esperado = tc1_FilterNombre
                // Le pongo null! para que no salte el warning del compilador, esto le dice al compilador que ya sé que es null.
                new object[] { "Modelo 3D A", null!, tc1_FilterNombre }, 
                
                // caso 2: filterNombre = null, filterCategoria = "Oficina", output esperado = tc2_FilterCategoria
                new object[] { null!, "Oficina", tc2_FilterCategoria } 
            };
            return allTests;
        }
    }
}