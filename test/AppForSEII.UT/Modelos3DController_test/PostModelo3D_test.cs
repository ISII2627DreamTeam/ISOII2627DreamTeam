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
using Xunit.Sdk;

namespace AppForSEII.UT.Modelos3DController_test
{
    public class PostModelo3D_test : AppForSEII4SqliteUT
    {
        public PostModelo3D_test()
        {
            // constructor vacío porque para el POST partimos de un estado limpio
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task PostModelo3D_Created_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<Modelos3DController>>();
            var controller = new Modelos3DController(_context, mockLogger.Object);

            // construcción del DTO de ENTRADA utilizando constructores
            var licenciaEntrada = new LicenciaModelo3DDTO(0, "Licencia Estándar", new DateTime(2028, 12, 31));
            var listaLicenciasEntrada = new List<LicenciaModelo3DDTO>();
            listaLicenciasEntrada.Add(licenciaEntrada);

            var nuevoModeloDTO = new Modelo3DDetailDTO(0, "Figura Batman 3D", "Superhéroes", (FormatoModelo3D)1, 19.99m, listaLicenciasEntrada);

            // Act
            var result = await controller.PostModelo3D(nuevoModeloDTO);

            // Assert
            //vVerificamos que devuelve HTTP 201 CreatedAtAction
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            
            // verificamos que el valor contenido es del tipo correcto
            var actualModeloDTO = Assert.IsType<Modelo3DDetailDTO>(createdResult.Value);
            
            // construcción del DTO ESPERADO simulando la asignación del ID = 1 por la base de datos
            var licenciaEsperada = new LicenciaModelo3DDTO(1, "Licencia Estándar", new DateTime(2028, 12, 31));
            var listaLicenciasEsperada = new List<LicenciaModelo3DDTO>();
            listaLicenciasEsperada.Add(licenciaEsperada);
            
            var expectedModeloDTO = new Modelo3DDetailDTO(1, "Figura Batman 3D", "Superhéroes", (FormatoModelo3D)1, 19.99m, listaLicenciasEsperada);
            
            // verificación final usando el método Equals.
            Assert.Equal(expectedModeloDTO, actualModeloDTO);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task PostModelo3D_BadRequest_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<Modelos3DController>>();
            var controller = new Modelos3DController(_context, mockLogger.Object);

            // construimos un DTO con la lista de licencias vacía mediante constructores
            var listaLicenciasVacia = new List<LicenciaModelo3DDTO>();
            var modeloInvalidoDTO = new Modelo3DDetailDTO(0, "Figura Fallida", "Errores", (FormatoModelo3D)1, 10.0m, listaLicenciasVacia);

            // simulamos el fallo del ModelState como lo haría la validación real del sistema
            controller.ModelState.AddModelError("Licencias", "El modelo debe tener al menos una licencia asociada.");

            // Act
            var result = await controller.PostModelo3D(modeloInvalidoDTO);

            // Assert
            // verificamos explícitamente que devuelve un 400 Bad Request
            Assert.IsType<BadRequestObjectResult>(result);
        }

        //EXAMEN
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task PostModelo3D_BadRequestNombreMal_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<Modelos3DController>>();
            var controller = new Modelos3DController(_context, mockLogger.Object);
            var expectedError = "¡Error! Cliente no es un nombre válido";
            

            // construimos un DTO con la lista de licencias vacía mediante constructores
            var listaLicenciasValido = new List<LicenciaModelo3DDTO>();
            listaLicenciasValido.Add(new LicenciaModelo3DDTO(0, "Laura", new DateTime(2028,12,31)));

            // simulamos el fallo del ModelState como lo haría la validación real del sistema
            var modeloInvalidoDTO = new Modelo3DDetailDTO(1, "Cliente", "Categoria B", (FormatoModelo3D)2, 5.0m, listaLicenciasValido);

            // Act
            var result = await controller.PostModelo3D(modeloInvalidoDTO);

            // Assert
            // verificamos explícitamente que devuelve un 400 Bad Request
            var createdResult= Assert.IsType<BadRequestObjectResult>(result);
            
            Assert.Equal(expectedError, createdResult.Value);
        }
    }
}