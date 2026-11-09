using System;

namespace AppForSEII.API.DTOs.PiezaDetalleDTOs
{
    // DTO auxiliar que representa los datos de un material compatible dentro de PiezaDetalleDTO.
    public class MaterialParaDetalleDTO
    {
        // Constructor vacío por defecto.
        public MaterialParaDetalleDTO()
        {
            Nombre = string.Empty;
        }

        // Constructor parametrizado usado al proyectar los materiales válidos de una pieza en GetPiezaDetalle.
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

        // Método Equals necesario para que SequenceEqual() en PiezaDetalleDTO pueda comparar 
        // si dos listas de materiales contienen exactamente los mismos materiales con sus precios y stocks.
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