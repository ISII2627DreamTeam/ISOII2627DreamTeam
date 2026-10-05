using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class Pieza3D
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Peso { get; set; }
    public CategoriaPieza Categoria { get; set; }

    public IList<Material> MaterialesValidos { get; set; } = new List<Material>();

    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}