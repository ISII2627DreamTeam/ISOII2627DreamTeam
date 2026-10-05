using System;
using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class EncargoImpresion
{
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