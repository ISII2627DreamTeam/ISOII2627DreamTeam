using System;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs
{
    public class Impresora3DDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        public TipoImpresora Tipo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public decimal PrecioKilovatioHora { get; set; }

        public decimal PrecioReserva { get; set; }

        
        public Impresora3DDTO()
        {
            Nombre = string.Empty;
            Modelo = string.Empty;
            Descripcion = string.Empty;
        }

        
        public Impresora3DDTO(Impresora3D impresora) : this()
        {
            Id = impresora.Id;
            Nombre = impresora.Nombre;
            Modelo = impresora.Modelo;
            Tipo = impresora.Tipo;
            Descripcion = impresora.Descripcion;
            PrecioKilovatioHora = impresora.PrecioKilovatioHora;
            PrecioReserva = impresora.PrecioReserva;
        }

        
        public override bool Equals(object? obj)
        {
            return obj is Impresora3DDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Modelo == dTO.Modelo &&
                   Tipo == dTO.Tipo &&
                   Descripcion == dTO.Descripcion &&
                   PrecioKilovatioHora == dTO.PrecioKilovatioHora &&
                   PrecioReserva == dTO.PrecioReserva;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Modelo, Tipo, Descripcion, PrecioKilovatioHora, PrecioReserva);
        }
    }
}