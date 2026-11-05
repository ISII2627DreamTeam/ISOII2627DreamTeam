using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.EncargoParaCrearDTOs
{
    public class EncargoParaCrearDTO
    {
        public EncargoParaCrearDTO()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionEnvio = string.Empty;
            NumeroTelefono = string.Empty;
            Descripcion = string.Empty;
            LineasEncargo = new List<LineaEncargoParaCrearDTO>();
        }

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

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es obligatorio")]
        public string NombreCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Los apellidos son obligatorios")]
        public string ApellidosCliente { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La dirección de envío es obligatoria")]
        public string DireccionEnvio { get; set; }

        public string NumeroTelefono { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public IList<LineaEncargoParaCrearDTO> LineasEncargo { get; set; }

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

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCliente, ApellidosCliente, DireccionEnvio, NumeroTelefono, Descripcion, MetodoPago, LineasEncargo);
        }
    }
}