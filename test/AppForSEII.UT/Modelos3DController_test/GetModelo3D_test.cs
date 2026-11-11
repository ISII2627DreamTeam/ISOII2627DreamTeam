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
    public class GetModelo3D_test : AppForSEII4SqliteUT
    {
        // el constructor preparalos datos en la base de datos SQLite en memoria antes de cada [Fact]
        public GetModelo3D_test()
        {
            // instanciamos la entidad principal usando el constructor estricto
            var modeloPrueba = new Modelo3D("Figura Coleccionable", "Figuras", (FormatoModelo3D)1, 25.99m);
            
            // instanciamos las entidades dependientes referenciando al objeto principal
            var licencia1 = new LicenciaModelo3D("Licencia Comercial", new DateTime(2027, 12, 31), modeloPrueba);
            var licencia2 = new LicenciaModelo3D("Licencia Personal", new DateTime(2028, 12, 31), modeloPrueba);
            
            // se asigna la lista sin utilizar inicializadores de objeto {}
            var listaLicencias = new List<LicenciaModelo3D>();
            listaLicencias.Add(licencia1);
            listaLicencias.Add(licencia2);
            modeloPrueba.Licencias = listaLicencias;

            // se inserta en el contexto de SQLite y se guarda
            _context.Modelos3D.Add(modeloPrueba);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelo3D_Found_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<Modelos3DController>>();
            var controller = new Modelos3DController(_context, mockLogger.Object);

            // construcción estricta del DTO esperado para la comprobación final (sin llaves de inicialización)
            var expectedLicencia1 = new LicenciaModelo3DDTO(1, "Licencia Comercial", new DateTime(2027, 12, 31));
            var expectedLicencia2 = new LicenciaModelo3DDTO(2, "Licencia Personal", new DateTime(2028, 12, 31));
            
            var expectedLicenciasList = new List<LicenciaModelo3DDTO>();
            expectedLicenciasList.Add(expectedLicencia1);
            expectedLicenciasList.Add(expectedLicencia2);

            // IMPORTANTE: Asegúrate de que los parámetros coinciden exactamente con el constructor de tu DTO
            var expectedModeloDTO = new Modelo3DDetailDTO(1, "Figura Coleccionable", "Figuras", (FormatoModelo3D)1, 25.99m, expectedLicenciasList);

            // Act
            var result = await controller.GetModelo3D(1);

            // Assert
            // verificamos que el tipo de respuesta HTTP es 200 OK
            var okResult = Assert.IsType<OkObjectResult>(result);
            
            // verificamos que el objeto contenido en la respuesta es del tipo Modelo3DDetailDTO
            var actualModeloDTO = Assert.IsType<Modelo3DDetailDTO>(okResult.Value);
            
            // verificamos mediante Equals que los datos devueltos coinciden con los esperados 
            Assert.Equal(expectedModeloDTO, actualModeloDTO);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelo3D_NotFound_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<Modelos3DController>>();
            var controller = new Modelos3DController(_context, mockLogger.Object);

            // Act
            // se busca un ID que no existe en el contexto semilla generado en el constructor
            var result = await controller.GetModelo3D(99);

            // Assert
            // se verifica que el tipo de respuesta es NotFoundResult (HTTP 404)
            Assert.IsType<NotFoundResult>(result);
        }
    }
}