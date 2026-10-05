using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(EncargoImpresionId), nameof(PiezaId))]
public class LineaEncargo
{
    public int Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal Subtotal { get; set; }

    
    public int EncargoImpresionId { get; set; }
    public EncargoImpresion EncargoImpresion { get; set; }

    
    public int PiezaId { get; set; }
    public Pieza3D Pieza { get; set; }

    
    public int MaterialId { get; set; }
    public Material MaterialSeleccionado { get; set; }
}