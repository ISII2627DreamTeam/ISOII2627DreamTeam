using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.EncargoParaCrearDTOs
{
    public class LineaEncargoParaCrearDTO
    {
        public LineaEncargoParaCrearDTO()
        {
        }

        public LineaEncargoParaCrearDTO(int piezaId, int materialId, int cantidad)
        {
            PiezaId = piezaId;
            MaterialId = materialId;
            Cantidad = cantidad;
        }

        [Required]
        public int PiezaId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is LineaEncargoParaCrearDTO dTO &&
                   PiezaId == dTO.PiezaId &&
                   MaterialId == dTO.MaterialId &&
                   Cantidad == dTO.Cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PiezaId, MaterialId, Cantidad);
        }
    }
}