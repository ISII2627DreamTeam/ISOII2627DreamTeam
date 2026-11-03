using System;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class AccesorioDTO
    {
        [StringLength(100)]
        public string Nombre { get; set; }
        
        public CategoriaAccesorio Categoria { get; set; }

        public AccesorioDTO(Accesorio accesorio)
        {
            Nombre = accesorio.Nombre;
            Categoria = accesorio.Categoria;
        }

        public override bool Equals(object? obj)
        {
            return obj is AccesorioDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Categoria == dTO.Categoria;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Categoria);
        }
    }
}