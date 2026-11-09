using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class CompraAccesorioDTO
    {
        // 1. DATA ANNOTATIONS (Validación automática)
        // Estas etiquetas son las que hacen que 'ModelState.ErrorCount > 0' se active en el controlador.
        // Si el cliente envía un POST sin 'NombreCliente', ASP.NET lo detecta automáticamente antes de ejecutar tu código.
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

        // CONSTRUCTOR A: Vacío
        // ASP.NET Core lo necesita obligatoriamente. Cuando el servidor recibe el JSON del POST, 
        // internamente usa este constructor para instanciar la clase vacía y luego rellenarla.
        public CompraAccesorioDTO() 
        { 
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionEnvio = string.Empty;
            NumeroTelefono = string.Empty;
            Lineas = new List<LineaCompraAccesorioDTO>();
        }

        // CONSTRUCTOR B: Para datos de entrada en los Tests (POST)
        // Lo has creado para la fase 'Arrange' de tus Unit Tests. Te permite crear el objeto 
        // que simula lo que envía el usuario SIN usar llaves {}, evitando la penalización de -1 punto.
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

        // CONSTRUCTOR C: Para el resultado esperado en los Tests (Assert)
        // Lo usas para construir el 'expectedDto' simulando que ya pasó por la base de datos 
        // (por eso este sí incluye el 'id' y la 'fechaCompra').
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

        // CONSTRUCTOR D: Mapeo desde Entidad (Para el GET)
        // Es el que llama el '.Select(c => new CompraAccesorioDTO(c))' en tu controlador.
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
        { // Compara propiedad por propiedad. Si no haces esto, Assert.Equal compara espacios de memoria y falla.
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