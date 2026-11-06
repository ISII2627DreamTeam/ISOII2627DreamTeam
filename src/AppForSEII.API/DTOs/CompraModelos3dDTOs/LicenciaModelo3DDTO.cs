using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.CompraModelos3DDTOs
{
    public class LicenciaModelo3DDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la licencia es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es obligatoria")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime FechaExpiracion { get; set; }

        // constructor vacío (Obligatorio para el POST)
        public LicenciaModelo3DDTO()
        {
            Nombre = string.Empty;
        }

        // constructor parametrizado (Obligatorio para el GET Details)
        public LicenciaModelo3DDTO(int id, string nombre, DateTime fechaExpiracion)
        {
            Id = id;
            Nombre = nombre;
            FechaExpiracion = fechaExpiracion;
        }

        public override bool Equals(object? obj)
        {
            return obj is LicenciaModelo3DDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   FechaExpiracion == dTO.FechaExpiracion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, FechaExpiracion);
        }
    }
}