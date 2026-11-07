using System;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class LineaCompraAccesorioDTO
    {
        public int AccesorioId { get; set; }
        
        public string NombreAccesorio { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
        
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }

        public LineaCompraAccesorioDTO() 
        { 
            NombreAccesorio = string.Empty;
        }

        // Constructor desde entidad (Para el GET)
        public LineaCompraAccesorioDTO(LineaCompraAccesorio linea)
        {
            AccesorioId = linea.AccesorioId;
            NombreAccesorio = linea.Accesorio.Nombre;
            Cantidad = linea.Cantidad;
            PrecioUnidad = linea.PrecioUnidad;
            Subtotal = linea.Subtotal;
        }

        public override bool Equals(object? obj)
        {
            return obj is LineaCompraAccesorioDTO dTO &&
                   AccesorioId == dTO.AccesorioId &&
                   NombreAccesorio == dTO.NombreAccesorio &&
                   Cantidad == dTO.Cantidad &&
                   PrecioUnidad == dTO.PrecioUnidad &&
                   Subtotal == dTO.Subtotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccesorioId, NombreAccesorio, Cantidad, PrecioUnidad, Subtotal);
        }
    }
}