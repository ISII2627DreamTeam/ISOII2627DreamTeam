using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class CompraAccesorioDetailDTO
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string DireccionEnvio { get; set; }
        public string NumeroTelefono { get; set; }
        public decimal PrecioTotal { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public List<LineaCompraAccesorioDTO> Lineas { get; set; }

        // El profesor pide pasar la entidad principal
        public CompraAccesorioDetailDTO(CompraAccesorios compra)
        {
            Id = compra.Id;
            FechaCompra = compra.FechaCompra;
            NombreCliente = compra.NombreCliente;
            ApellidosCliente = compra.ApellidosCliente;
            DireccionEnvio = compra.DireccionEnvio;
            NumeroTelefono = compra.NumeroTelefono;
            PrecioTotal = compra.PrecioTotal;
            MetodoPago = compra.MetodoPago;
            // Mapeamos la lista llamando al DTO de las líneas
            Lineas = compra.LineasCompra.Select(l => new LineaCompraAccesorioDTO(l)).ToList();
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraAccesorioDetailDTO dTO &&
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