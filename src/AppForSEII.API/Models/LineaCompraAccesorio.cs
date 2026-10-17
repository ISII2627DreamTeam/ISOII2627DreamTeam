using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(AccesorioId), nameof(CompraAccesoriosId))]
public class LineaCompraAccesorio
{
    public LineaCompraAccesorio() 
    { 
    }
    public LineaCompraAccesorio(int accesorioId, int compraAccesoriosId, int cantidad, decimal precioUnidad)
    {
        AccesorioId = accesorioId;
        CompraAccesoriosId = compraAccesoriosId;
        Cantidad = cantidad;
        PrecioUnidad = precioUnidad;
        Subtotal = cantidad * precioUnidad; // Se calcula solo
    }
    

    public int AccesorioId { get; set; }

    public Accesorio Accesorio { get; set; }

    public int CompraAccesoriosId { get; set; }

    public CompraAccesorios CompraAccesorios { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required]
    [Precision(8, 2)]
    public decimal PrecioUnidad { get; set; }

    [Required]
    [Precision(10, 2)]
    public decimal Subtotal { get; set; }
}