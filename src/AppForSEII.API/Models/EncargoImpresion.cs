using System;
using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class EncargoImpresion
{
    public EncargoImpresion()
    {
        NombreCliente = string.Empty;
        ApellidosCliente = string.Empty;
        DireccionEnvio = string.Empty;
        NumeroTelefono = string.Empty;
        Descripcion = string.Empty;
        Cliente = new Cliente();
        ClienteId = string.Empty;
    }

    public EncargoImpresion( DateTime fechaEncargo, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, string descripcion, decimal precioTotal, MetodoPago metodoPago, Cliente cliente)
    {
        FechaEncargo = fechaEncargo;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionEnvio = direccionEnvio;
        NumeroTelefono = numeroTelefono;
        Descripcion = descripcion;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        ClienteId = cliente.Id;
        Cliente = cliente;
    }

public EncargoImpresion(DateTime fechaEncargo, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, string descripcion, decimal precioTotal, MetodoPago metodoPago, IList<LineaEncargo> lineasEncargo)
    {
        FechaEncargo = fechaEncargo;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionEnvio = direccionEnvio;
        NumeroTelefono = numeroTelefono;
        Descripcion = descripcion;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        LineasEncargo = lineasEncargo;
        Cliente = new Cliente();
        ClienteId = string.Empty;
    }

    public int Id { get; set; }
    public DateTime FechaEncargo { get; set; }

    public string NombreCliente { get; set; }

    public string ApellidosCliente { get; set; }
    public string DireccionEnvio { get; set; }
    public string NumeroTelefono { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioTotal { get; set; }
    public MetodoPago MetodoPago { get; set; }

    
    public string ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    
    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}