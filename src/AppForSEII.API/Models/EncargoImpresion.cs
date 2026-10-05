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

    // --- Relación con Cliente ---
    public string ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    // --- COMUNICACIÓN CON LINEA ENCARGO (1 a Muchos) ---
    // Un encargo contiene una lista de líneas de encargo
    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}