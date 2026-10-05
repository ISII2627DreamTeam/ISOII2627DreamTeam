namespace AppForSEII.API.Models;

public class LineaReserva
{
    public LineaReserva()
    {
    }

    public LineaReserva(int id, TiempoReserva tiempoReserva, decimal precioSubtotal, Impresora3D impresora)
    {
        Id = id;
        TiempoReserva = tiempoReserva;
        PrecioSubtotal = precioSubtotal;
        Impresora = impresora;
    }

    public int Id { get; set; }
    public TiempoReserva TiempoReserva { get; set; }
    public decimal PrecioSubtotal { get; set; }
    

    public Impresora3D Impresora { get; set; }
}