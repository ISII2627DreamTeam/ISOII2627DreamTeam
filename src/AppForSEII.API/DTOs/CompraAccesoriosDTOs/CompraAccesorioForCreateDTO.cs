using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class CompraAccesorioForCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string NombreCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string ApellidosCliente { get; set; }

        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroTelefono { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public List<LineaCompraAccesorioForCreateDTO> Lineas { get; set; }

        public CompraAccesorioForCreateDTO()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionEnvio = string.Empty;
            NumeroTelefono = string.Empty;
            Lineas = new List<LineaCompraAccesorioForCreateDTO>();
        }

        public CompraAccesorioForCreateDTO(string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, MetodoPago metodoPago, List<LineaCompraAccesorioForCreateDTO> lineas)
        {
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            MetodoPago = metodoPago;
            Lineas = lineas;
        }
    }
}