using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.EncargoImpresionDTOs
{
    public class PiezaParaEncargarDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; }

        [Required]
        public string Categoria { get; set; }

        [Required]
        public decimal Peso { get; set; }

        public PiezaParaEncargarDTO(int id, string nombre, string categoria, decimal peso)
        {
            Id = id;
            Nombre = nombre;
            Categoria = categoria;
            Peso = peso;
        }

        public override bool Equals(object? obj)
        {
            return obj is PiezaParaEncargarDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Categoria == dTO.Categoria &&
                   Peso == dTO.Peso;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Categoria, Peso);
        }
    }
}