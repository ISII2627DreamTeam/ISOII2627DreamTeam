using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.EncargoParaCrearDTOs
{
    // DTO (Data Transfer Object) principal que representa la petición de entrada y salida 
    // del método POST (PostEncargoImpresion) para crear un nuevo encargo de impresión 3D.
    public class EncargoParaCrearDTO
    {
        // Constructor vacío necesario para la deserialización automática de JSON en ASP.NET Core
        // cuando el cliente envía los datos en el cuerpo (Body) de la petición HTTP POST.
        // Inicializa las cadenas a vacío y la lista para evitar excepciones de referencia nula (NullReferenceException).
        public EncargoParaCrearDTO()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionEnvio = string.Empty;
            NumeroTelefono = string.Empty;
            Descripcion = string.Empty;
            LineasEncargo = new List<LineaEncargoParaCrearDTO>();
        }

        // Constructor parametrizado obligatorio según la rúbrica de la asignatura (evita penalización de -1 punto).
        // Se utiliza en las pruebas unitarias y en el controlador para instanciar el DTO de forma directa
        // sin recurrir a inicializadores de llaves { }.
        public EncargoParaCrearDTO(string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, string descripcion, MetodoPago metodoPago, IList<LineaEncargoParaCrearDTO> lineasEncargo)
        {
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            Descripcion = descripcion;
            MetodoPago = metodoPago;
            LineasEncargo = lineasEncargo;
        }

        // Data Annotation [Required]: obliga a que el campo viaje en el JSON.
        // AllowEmptyStrings = false impide que se envíe una cadena en blanco ("").
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio")]
        public string NombreCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Los apellidos son obligatorios")]
        public string ApellidosCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La dirección de envío es obligatoria")]
        public string DireccionEnvio { get; set; }

        // Propiedades opcionales de contacto e instrucciones adicionales del encargo.
        public string NumeroTelefono { get; set; }

        public string Descripcion { get; set; }

        // Tipo enumerado (Enum) que indica la forma de pago elegida por el cliente.
        [Required]
        public MetodoPago MetodoPago { get; set; }

        // Colección de líneas del pedido (cada línea contiene qué pieza, qué material y cuántas unidades se encargan).
        [Required]
        public IList<LineaEncargoParaCrearDTO> LineasEncargo { get; set; }

        // Sobrescritura del método Equals exigida por la rúbrica (evita penalización de -1 punto).
        // Permite que xUnit compare dos instancias de EncargoParaCrearDTO por el valor de sus propiedades 
        // (y no por su dirección de memoria) cuando ejecutamos Assert.Equal(expected, actual) en los tests.
        // IMPORTANTE: Para comparar listas (LineasEncargo) no sirve '==', se debe usar SequenceEqual(),
        // que comprueba elemento a elemento llamando al Equals() de LineaEncargoParaCrearDTO.
        public override bool Equals(object? obj)
        {
            return obj is EncargoParaCrearDTO dTO &&
                   NombreCliente == dTO.NombreCliente &&
                   ApellidosCliente == dTO.ApellidosCliente &&
                   DireccionEnvio == dTO.DireccionEnvio &&
                   NumeroTelefono == dTO.NumeroTelefono &&
                   Descripcion == dTO.Descripcion &&
                   MetodoPago == dTO.MetodoPago &&
                   LineasEncargo.SequenceEqual(dTO.LineasEncargo);
        }

        // Genera un código hash único basado en los valores de las propiedades del DTO.
        // Siempre debe sobrescribirse en C# cuando se sobrescribe el método Equals().
        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCliente, ApellidosCliente, DireccionEnvio, NumeroTelefono, Descripcion, MetodoPago, LineasEncargo);
        }
    }
}