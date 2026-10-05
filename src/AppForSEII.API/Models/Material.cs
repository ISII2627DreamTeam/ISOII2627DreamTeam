using System.Collections.Generic;

namespace AppForSEII.API.Models;

public class Material
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal PrecioPorGramo { get; set; }
    public decimal StockGramos { get; set; }

    // --- COMUNICACIÓN CON PIEZAS 3D (Relación muchos a muchos) ---
    // Un material puede ser válido para muchas piezas
    public HashSet<Pieza3D> Piezas3D { get; set; } = new HashSet<Pieza3D>();

    // --- COMUNICACIÓN CON LINEA ENCARGO ---
    // Un material puede haber sido seleccionado en muchas líneas de encargo
    public IList<LineaEncargo> LineasEncargo { get; set; } = new List<LineaEncargo>();
}