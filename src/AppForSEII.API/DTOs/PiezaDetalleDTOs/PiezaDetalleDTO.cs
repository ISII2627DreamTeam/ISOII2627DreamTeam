using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.PiezaDetalleDTOs
{
    // DTO de salida para el método GET de detalle (GetPiezaDetalle).
    // Devuelve toda la información de una pieza específica junto con la lista de materiales compatibles.
    public class PiezaDetalleDTO
    {
        // Constructor por defecto sin parámetros que inicializa el string y la lista vacía
        // para prevenir errores de referencias nulas (NullReferenceException).
        public PiezaDetalleDTO()
        {
            Nombre = string.Empty;
            MaterialesValidos = new List<MaterialParaDetalleDTO>();
        }

        // Constructor con todos los parámetros exigido por la rúbrica para construir el objeto
        // en la consulta LINQ del controlador y en los casos de prueba unitarios.
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

        // Relación anidada: lista de DTOs con los detalles de cada material compatible con esta pieza.
        public IList<MaterialParaDetalleDTO> MaterialesValidos { get; set; }

        // Sobrescritura de Equals para las pruebas unitarias (GetPiezaDetalle_test).
        // Compara los campos simples con '==' y la colección de materiales con SequenceEqual(),
        // que a su vez invoca el método Equals() de MaterialParaDetalleDTO para cada elemento.
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