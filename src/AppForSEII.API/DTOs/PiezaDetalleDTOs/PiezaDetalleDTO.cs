using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.PiezaDetalleDTOs
{
    public class PiezaDetalleDTO
    {
        public PiezaDetalleDTO()
        {
            Nombre = string.Empty;
            MaterialesValidos = new List<MaterialParaDetalleDTO>();
        }

        public PiezaDetalleDTO(int id, string nombre, decimal peso, CategoriaPieza categoria, IList<MaterialParaDetalleDTO> materialesValidos)
        {
            Id = id;
            Nombre = nombre;
            Peso = peso;
            Categoria = categoria;
            MaterialesValidos = materialesValidos;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Peso { get; set; }
        public CategoriaPieza Categoria { get; set; }
        public IList<MaterialParaDetalleDTO> MaterialesValidos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PiezaDetalleDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Peso == dTO.Peso &&
                   Categoria == dTO.Categoria &&
                   MaterialesValidos.SequenceEqual(dTO.MaterialesValidos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Peso, Categoria, MaterialesValidos);
        }
    }
}