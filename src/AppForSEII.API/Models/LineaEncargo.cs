using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(EncargoImpresionId), nameof(PiezaId))]
public class LineaEncargo
{

    public LineaEncargo()
    {
        EncargoImpresion = new EncargoImpresion();
        Pieza = new Pieza3D();
        MaterialSeleccionado = new Material();
    }

    public LineaEncargo(int cantidad, decimal precioUnidad, decimal subtotal, EncargoImpresion encargoImpresion, Pieza3D pieza, Material materialSeleccionado)
    {
        Cantidad = cantidad;
        PrecioUnidad = precioUnidad;
        Subtotal = subtotal;
        EncargoImpresion = encargoImpresion;
        EncargoImpresionId = encargoImpresion.Id;
        Pieza = pieza;
        PiezaId = pieza.Id;
        MaterialSeleccionado = materialSeleccionado;
        MaterialId = materialSeleccionado.Id;
    }

    public int Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal Subtotal { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int EncargoImpresionId { get; set; }
    public EncargoImpresion EncargoImpresion { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int PiezaId { get; set; }
    public Pieza3D Pieza { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int MaterialId { get; set; }
    public Material MaterialSeleccionado { get; set; }
}