using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(AccesorioId), nameof(CompraAccesoriosId))]
public class LineaCompraAccesorio
{
    public LineaCompraAccesorio()
    {
        Accesorio = new Accesorio();
        CompraAccesorios = new CompraAccesorios();
        AccesorioId = -1;
        CompraAccesoriosId = -1;
    }
    public LineaCompraAccesorio(Accesorio accesorio, CompraAccesorios compraAccesorios, int cantidad, decimal precioUnidad) : this()
    {
        AccesorioId = accesorio.Id;
        Accesorio = accesorio;
        CompraAccesoriosId = compraAccesorios.Id;
        CompraAccesorios = compraAccesorios;
        Cantidad = cantidad;
        PrecioUnidad = precioUnidad;
        Subtotal = cantidad * precioUnidad;
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