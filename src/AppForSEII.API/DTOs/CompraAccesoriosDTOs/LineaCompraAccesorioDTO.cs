using System;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class LineaCompraAccesorioDTO
    {
        // Este DTO representa una línea individual dentro de la lista de compras.
        public int AccesorioId { get; set; }
        
        public string NombreAccesorio { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
        
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }

        // CONSTRUCTOR A: Vacío (Obligatorio para ASP.NET)
        // Usado por el framework para deserializar el JSON vacío inicial.
        public LineaCompraAccesorioDTO() 
        { 
            NombreAccesorio = string.Empty;
        }

        // CONSTRUCTOR B: Para los datos de entrada del cliente (POST en los Tests)
        // Fíjate que SOLO pide ID y Cantidad.
        public LineaCompraAccesorioDTO(int accesorioId, int cantidad) 
        {
            AccesorioId = accesorioId;
            Cantidad = cantidad;
            NombreAccesorio = string.Empty;
        }

        // CONSTRUCTOR C: Para comprobar el resultado exacto en los Assert de los Tests
        // Este sí tiene todos los campos, porque simula la respuesta final del servidor 
        // una vez que ya ha calculado los precios reales.
        public LineaCompraAccesorioDTO(int accesorioId, string nombreAccesorio, int cantidad, decimal precioUnidad, decimal subtotal) 
        {
            AccesorioId = accesorioId;
            NombreAccesorio = nombreAccesorio;
            Cantidad = cantidad;
            PrecioUnidad = precioUnidad;
            Subtotal = subtotal;
        }

        // CONSTRUCTOR D: Mapeo desde Entidad (Para el GET)
        public LineaCompraAccesorioDTO(LineaCompraAccesorio linea)
        {
            AccesorioId = linea.AccesorioId;
            NombreAccesorio = linea.Accesorio.Nombre;
            Cantidad = linea.Cantidad;
            PrecioUnidad = linea.PrecioUnidad;
            Subtotal = linea.Subtotal;
        }

        public override bool Equals(object? obj)
        { // Compara que todas las propiedades numéricas y de texto coincidan exactamente.
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