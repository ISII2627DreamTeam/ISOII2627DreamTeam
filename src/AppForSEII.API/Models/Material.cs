using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class Material
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal PrecioPorGramo { get; set; }
    public decimal StockGramos { get; set; }

    public IList<Pieza3D> Piezas3D { get; set; } = new List<Pieza3D>();

    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}