using Microsoft.EntityFrameworkCore;          

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(Modelo3DID), nameof(CompraModelo3DID))]
public class LineaCompraModelo
{
    public LineaCompraModelo() 
    { 
        Modelo3D = new Modelo3D();
        CompraModelo3D = new CompraModelo3D();
    }

    public LineaCompraModelo(Modelo3D modelo3D, CompraModelo3D compraModelo3D, int cantidadLicencias, decimal precioUnidad) : this()
    {

        Modelo3DID = modelo3D.Id;
        CompraModelo3DID = compraModelo3D.Id;
        
        Modelo3D = modelo3D;
        CompraModelo3D = compraModelo3D;
        
        CantidadLicencias = cantidadLicencias;
        PrecioUnidad = precioUnidad;
        Subtotal = cantidadLicencias * precioUnidad; 
    }

    public int Modelo3DID { get; set; }
    public int CompraModelo3DID { get; set; }

    public Modelo3D Modelo3D { get; set; }
    public CompraModelo3D CompraModelo3D { get; set; }

    [Required]
    public int CantidadLicencias { get; set; }

    [Required]
    public decimal PrecioUnidad { get; set; }

    [Required]
    public decimal Subtotal { get; set; }
}