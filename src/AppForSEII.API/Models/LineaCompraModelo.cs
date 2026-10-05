using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(Modelo3DId), nameof(CompraModelo3DId))]
public class LineaCompraModelo
{
    public int Modelo3DId { get; set; }

    public Modelo3D Modelo3D { get; set; }

    public int CompraModelo3DId { get; set; }

    public CompraModelo3D CompraModelo3D { get; set; }

    public int CantidadLicencias { get; set; }

    public decimal PrecioUnidad { get; set; }

    public decimal Subtotal { get; set; }
}