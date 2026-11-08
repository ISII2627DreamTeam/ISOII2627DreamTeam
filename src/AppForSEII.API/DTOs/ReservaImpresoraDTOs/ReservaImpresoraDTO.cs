using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs
{
    public class ReservaImpresoraDTO
    {
        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string DireccionFacturacion { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public decimal PrecioTotal { get; set; }
        public List<LineaReservaDTO> Lineas { get; set; }

        
        public ReservaImpresoraDTO(ReservaImpresora reserva)
        {
            Id = reserva.Id;
            FechaReserva = reserva.FechaReserva;
            NombreCliente = reserva.NombreCliente;
            ApellidosCliente = reserva.ApellidosCliente;
            DireccionFacturacion = reserva.DireccionFacturacion;
            MetodoPago = reserva.MetodoPago;
            PrecioTotal = reserva.PrecioTotal;
            Lineas = reserva.LineasReserva.Select(l => new LineaReservaDTO(l)).ToList();
        }

        public override bool Equals(object? obj)
        {
            return obj is ReservaImpresoraDTO dTO &&
                   Id == dTO.Id &&
                   FechaReserva == dTO.FechaReserva &&
                   NombreCliente == dTO.NombreCliente &&
                   ApellidosCliente == dTO.ApellidosCliente &&
                   DireccionFacturacion == dTO.DireccionFacturacion &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal &&
                   Lineas.SequenceEqual(dTO.Lineas);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FechaReserva, NombreCliente, ApellidosCliente, DireccionFacturacion, MetodoPago, PrecioTotal);
        }
    }
}