using Microsoft.EntityFrameworkCore;          

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(Modelo3DId), nameof(CompraModelo3DId))]
public class LineaCompraModelo
{
    public LineaCompraModelo() { }

    public LineaCompraModelo(int modelo3DId, int compraModelo3DId, int cantidadLicencias, decimal precioUnidad)
    {
        Modelo3DId = modelo3DId;
        CompraModelo3DId = compraModelo3DId;
        CantidadLicencias = cantidadLicencias;
        PrecioUnidad = precioUnidad;
        Subtotal = cantidadLicencias * precioUnidad; // Calculado el subtotal sin hacer nada.
    }
    public int Modelo3DId { get; set; }

    public Modelo3D Modelo3D { get; set; }

    public int CompraModelo3DId { get; set; }

    public CompraModelo3D CompraModelo3D { get; set; }

    [Required]
    public int CantidadLicencias { get; set; }

    [Required]
    public decimal PrecioUnidad { get; set; }

    [Required]
    public decimal Subtotal { get; set; }
}