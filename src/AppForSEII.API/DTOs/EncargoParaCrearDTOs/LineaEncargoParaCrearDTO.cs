using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.EncargoParaCrearDTOs
{
    // DTO secundario que representa cada una de las líneas individuales de un encargo de impresión
    // dentro de la lista LineasEncargo de EncargoParaCrearDTO.
    public class LineaEncargoParaCrearDTO
    {
        // Constructor por defecto sin parámetros requerido por el framework para deserializar el JSON.
        public LineaEncargoParaCrearDTO()
        {
        }

        // Constructor con todos los parámetros para instanciar cada línea del pedido cumpliendo la rúbrica
        // (sin usar llaves inicializadoras { }).
        public LineaEncargoParaCrearDTO(int piezaId, int materialId, int cantidad)
        {
            PiezaId = piezaId;
            MaterialId = materialId;
            Cantidad = cantidad;
        }

        // Identificador de la pieza 3D que el cliente desea imprimir (clave foránea hacia Pieza3D).
        [Required]
        public int PiezaId { get; set; }

        // Identificador del material seleccionado para imprimir dicha pieza (clave foránea hacia Material).
        [Required]
        public int MaterialId { get; set; }

        // Número de unidades a imprimir. 
        // [Range(1, int.MaxValue)] valida que la cantidad sea estrictamente positiva (mínimo 1 unidad).
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        // Sobrescritura de Equals indispensable para que el método SequenceEqual() del DTO padre 
        // (EncargoParaCrearDTO) pueda verificar si dos listas de líneas contienen exactamente 
        // las mismas piezas, materiales y cantidades durante las pruebas unitarias.
        public override bool Equals(object? obj)
        {
            return obj is LineaEncargoParaCrearDTO dTO &&
                   PiezaId == dTO.PiezaId &&
                   MaterialId == dTO.MaterialId &&
                   Cantidad == dTO.Cantidad;
        }

        // Cálculo del código hash combinando los tres atributos de la línea de encargo.
        public override int GetHashCode()
        {
            return HashCode.Combine(PiezaId, MaterialId, Cantidad);
        }
    }
}