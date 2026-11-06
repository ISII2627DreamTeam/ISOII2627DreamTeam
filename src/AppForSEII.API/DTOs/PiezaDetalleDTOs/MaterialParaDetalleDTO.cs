using System;

namespace AppForSEII.API.DTOs.PiezaDetalleDTOs
{
    public class MaterialParaDetalleDTO
    {
        public MaterialParaDetalleDTO()
        {
            Nombre = string.Empty;
        }

        public MaterialParaDetalleDTO(int id, string nombre, decimal precioPorGramo, decimal stockGramos)
        {
            Id = id;
            Nombre = nombre;
            PrecioPorGramo = precioPorGramo;
            StockGramos = stockGramos;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioPorGramo { get; set; }
        public decimal StockGramos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is MaterialParaDetalleDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   PrecioPorGramo == dTO.PrecioPorGramo &&
                   StockGramos == dTO.StockGramos;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, PrecioPorGramo, StockGramos);
        }
    }
}