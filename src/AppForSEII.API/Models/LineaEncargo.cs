using Microsoft.EntityFrameworkCore;
namespace AppForSEII.API.Models;

[PrimaryKey(nameof(EncargoImpresionId), nameof(PiezaId))]
public class LineaEncargo
{
    public LineaEncargo()
    {
    }

    public LineaEncargo(int encargoImpresionId, int piezaId, int cantidad, decimal precioUnidad, decimal subtotal, Pieza3D pieza, Material materialSeleccionado)
    {
        EncargoImpresionId = encargoImpresionId;
        PiezaId = piezaId;
        Cantidad = cantidad;
        PrecioUnidad = precioUnidad;
        Subtotal = subtotal;
        Pieza = pieza;
        MaterialSeleccionado = materialSeleccionado;
    }


    public int Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal Subtotal { get; set; }


    public int PiezaId { get; set; }
    public Pieza3D Pieza { get; set; }
    

    public int EncargoImpresionId { get; set; }
    public EncargoImpresion EncargoImpresion { get; set; }

    public int MaterialId { get; set; } 
    public Material MaterialSeleccionado { get; set; }
}