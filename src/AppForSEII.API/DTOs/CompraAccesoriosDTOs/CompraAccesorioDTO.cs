using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class CompraAccesorioDTO
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string ApellidosCliente { get; set; }

        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroTelefono { get; set; }

        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public List<LineaCompraAccesorioDTO> Lineas { get; set; }

        // Constructor vacío (Obligatorio para recibir el POST)
        public CompraAccesorioDTO() 
        { 
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionEnvio = string.Empty;
            NumeroTelefono = string.Empty;
            Lineas = new List<LineaCompraAccesorioDTO>();
        }

        // Constructor para crear los datos de entrada del POST
        public CompraAccesorioDTO(string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, MetodoPago metodoPago, List<LineaCompraAccesorioDTO> lineas, decimal precioTotal) 
        {
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            MetodoPago = metodoPago;
            Lineas = lineas;
            PrecioTotal = precioTotal;
        }

        // Constructor para comprobar el resultado exacto en los Assert
        public CompraAccesorioDTO(int id, DateTime fechaCompra, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, MetodoPago metodoPago, List<LineaCompraAccesorioDTO> lineas, decimal precioTotal) 
        {
            Id = id;
            FechaCompra = fechaCompra;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            MetodoPago = metodoPago;
            Lineas = lineas;
            PrecioTotal = precioTotal;
        }

        // Constructor desde entidad (Para el GET)
        public CompraAccesorioDTO(CompraAccesorios compra)
        {
            Id = compra.Id;
            FechaCompra = compra.FechaCompra;
            NombreCliente = compra.NombreCliente;
            ApellidosCliente = compra.ApellidosCliente;
            DireccionEnvio = compra.DireccionEnvio;
            NumeroTelefono = compra.NumeroTelefono;
            PrecioTotal = compra.PrecioTotal;
            MetodoPago = compra.MetodoPago;
            Lineas = compra.LineasCompra.Select(l => new LineaCompraAccesorioDTO(l)).ToList();
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraAccesorioDTO dTO &&
                   Id == dTO.Id &&
                   FechaCompra == dTO.FechaCompra &&
                   NombreCliente == dTO.NombreCliente &&
                   ApellidosCliente == dTO.ApellidosCliente &&
                   DireccionEnvio == dTO.DireccionEnvio &&
                   NumeroTelefono == dTO.NumeroTelefono &&
                   PrecioTotal == dTO.PrecioTotal &&
                   MetodoPago == dTO.MetodoPago &&
                   Lineas.SequenceEqual(dTO.Lineas);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FechaCompra, NombreCliente, ApellidosCliente, DireccionEnvio, NumeroTelefono, PrecioTotal, MetodoPago);
        }
    }
}