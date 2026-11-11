using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models;

public class Material
{
    public Material()
    {
        Nombre = string.Empty;
    }

    public Material(string nombre, decimal precioPorGramo, decimal stockGramos)
    {
        Nombre = nombre;
        PrecioPorGramo = precioPorGramo;
        StockGramos = stockGramos;
    }

    public int Id { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; }

    [Precision(10, 4)]
    [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Fuera de rango")]
    public decimal PrecioPorGramo { get; set; }

    [Precision(10, 4)]
    [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Fuera de rango")]
    public decimal StockGramos { get; set; }

    public IList<Pieza3D> Piezas3D { get; set; } = new List<Pieza3D>();

    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}