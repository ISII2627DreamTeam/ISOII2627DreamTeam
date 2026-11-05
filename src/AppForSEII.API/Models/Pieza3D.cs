using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class Pieza3D
{
    public Pieza3D()
    {
        Nombre = string.Empty;
        Peso = 0;
        Categoria = new CategoriaPieza();
    }

    public Pieza3D(int id, string nombre, decimal peso, CategoriaPieza categoria)
    {
        Id = id;
        Nombre = nombre;
        Peso = peso;
        Categoria = categoria;
    }
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Peso { get; set; }
    public CategoriaPieza Categoria { get; set; }

    public IList<Material> MaterialesValidos { get; set; } = new List<Material>();

    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}