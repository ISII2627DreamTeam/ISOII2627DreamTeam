using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

[PrimaryKey(nameof(Id), nameof(ReservaImpresoraId))]
public class LineaReserva
{
    public LineaReserva()
    {
    }

    public LineaReserva(int id, TiempoReserva tiempoReserva, decimal precioSubtotal, int impresora3DId, int reservaImpresoraId)
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
    //public Impresora3D Impresora { get; set; }

    public int ReservaImpresoraId { get; set; }
    public ReservaImpresora ReservaImpresora { get; set; }
}