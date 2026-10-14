using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(Id), nameof(ReservaImpresoraId))]
public class LineaReserva
{
    public LineaReserva()
    {
        TiempoReserva = new TiempoReserva();
        PrecioSubtotal = 0;
        Impresora3DId = -1;
        ReservaImpresoraId = -1;
        Impresora = new Impresora3D();
        ReservaImpresora = new ReservaImpresora();
    }

    public LineaReserva(int id, TiempoReserva tiempoReserva, decimal precioSubtotal, int impresora3DId, int reservaImpresoraId)
    :this()
    {
        Id = id;
        TiempoReserva = tiempoReserva;
        PrecioSubtotal = precioSubtotal;
        Impresora3DId = impresora3DId;
        ReservaImpresoraId = reservaImpresoraId;
    }

    public int Id { get; set; }
    
    public TiempoReserva TiempoReserva { get; set; }
    
    public decimal PrecioSubtotal { get; set; }

    public int Impresora3DId { get; set; }
    public Impresora3D Impresora { get; set; }
    [ForeignKey(nameof(ReservaImpresora))]
    public int ReservaImpresoraId { get; set; }
    [Required]
    public ReservaImpresora ReservaImpresora { get; set; }
    
}