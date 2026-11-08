using System;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs
{
    public class LineaReservaDTO
    {
        [Required]
        public int Impresora3DId { get; set; }

        public string NombreImpresora { get; set; }
        public string Modelo { get; set; }
        public TipoImpresora Tipo { get; set; }
        public string Descripcion { get; set; }

        [Required]
        public TiempoReserva TiempoReserva { get; set; }

        public decimal PrecioKilovatioHora { get; set; }
        public decimal PrecioReserva { get; set; }
        public decimal PrecioSubtotal { get; set; }

        
        public LineaReservaDTO()
        {
            NombreImpresora = string.Empty;
            Modelo = string.Empty;
            Descripcion = string.Empty;
        }


        public LineaReservaDTO(LineaReserva linea)
        {
            Impresora3DId = linea.Impresora3DId;
            NombreImpresora = linea.Impresora.Nombre;
            Modelo = linea.Impresora.Modelo;
            Tipo = linea.Impresora.Tipo;
            Descripcion = linea.Impresora.Descripcion;
            TiempoReserva = linea.TiempoReserva;
            PrecioKilovatioHora = linea.Impresora.PrecioKilovatioHora;
            PrecioReserva = linea.Impresora.PrecioReserva;
            PrecioSubtotal = linea.PrecioSubtotal;
        }

        public override bool Equals(object? obj)
        {
            return obj is LineaReservaDTO dTO &&
                   Impresora3DId == dTO.Impresora3DId &&
                   NombreImpresora == dTO.NombreImpresora &&
                   Modelo == dTO.Modelo &&
                   Tipo == dTO.Tipo &&
                   Descripcion == dTO.Descripcion &&
                   TiempoReserva == dTO.TiempoReserva &&
                   PrecioKilovatioHora == dTO.PrecioKilovatioHora &&
                   PrecioReserva == dTO.PrecioReserva &&
                   PrecioSubtotal == dTO.PrecioSubtotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Impresora3DId, NombreImpresora, Modelo, Tipo, Descripcion, TiempoReserva,
                HashCode.Combine(PrecioKilovatioHora, PrecioReserva, PrecioSubtotal));
        }
    }
}