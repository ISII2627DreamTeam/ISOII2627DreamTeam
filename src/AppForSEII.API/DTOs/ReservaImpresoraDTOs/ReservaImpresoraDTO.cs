using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs
{
    public class ReservaImpresoraDTO
    {
        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
        [Required]
        [StringLength(50)]
        public string NombreCliente { get; set; }
        [Required]
        [StringLength(100)]
        public string ApellidosCliente { get; set; }
        [Required]
        [StringLength(200)]
        public string DireccionFacturacion { get; set; }
        [Required]
        public MetodoPago MetodoPago { get; set; }
        public decimal PrecioTotal { get; set; }
        [Required]
        public List<LineaReservaDTO> Lineas { get; set; }

        public ReservaImpresoraDTO()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionFacturacion = string.Empty;
            Lineas = new List<LineaReservaDTO>();
        }

        public ReservaImpresoraDTO(string nombreCliente, string apellidosCliente, string direccionFacturacion, MetodoPago metodoPago, List<LineaReservaDTO> lineas)
        {
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionFacturacion = direccionFacturacion;
            MetodoPago = metodoPago;
            Lineas = lineas;
        }

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