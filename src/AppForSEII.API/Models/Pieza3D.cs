using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class Pieza3D
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Peso { get; set; }
    public CategoriaPieza Categoria { get; set; }

    // --- COMUNICACIÓN CON MATERIALES ---
    // Una pieza tiene un conjunto (HashSet) de materiales válidos
    public HashSet<Material> MaterialesValidos { get; set; } = new HashSet<Material>();

    // --- COMUNICACIÓN CON LINEA ENCARGO (Inversa opcional, pero recomendada) ---
    // Una pieza puede estar en muchas líneas de encargo
    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}