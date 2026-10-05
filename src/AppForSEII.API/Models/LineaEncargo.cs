using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(EncargoImpresionId), nameof(PiezaId))]
public class LineaEncargo
{
    public int Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal Subtotal { get; set; }

    // --- COMUNICACIÓN CON ENCARGO ---
    public int EncargoImpresionId { get; set; }
    public EncargoImpresion EncargoImpresion { get; set; }

    // --- COMUNICACIÓN CON PIEZA ---
    public int PiezaId { get; set; }
    public Pieza3D Pieza { get; set; }

    // --- COMUNICACIÓN CON MATERIAL SELECCIONADO ---
    public int MaterialId { get; set; }
    public Material MaterialSeleccionado { get; set; }
}