namespace AppForSEII.API.Models;

public class CompraAccesorios
{
    public CompraAccesorios()
    {
    }

    public CompraAccesorios(int id, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, DateTime fechaCompra, MetodoPago metodoPago, decimal precioTotal)
    {
        Id = id;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionEnvio = direccionEnvio;
        NumeroTelefono = numeroTelefono;
        FechaCompra = fechaCompra;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
    }

    public int Id { get; set; }

    public DateTime FechaCompra { get; set; }

    public string NombreCliente { get; set; }

    public string ApellidosCliente { get; set; }

    public string DireccionEnvio { get; set; }

    public string NumeroTelefono { get; set; }

    public MetodoPago MetodoPago { get; set; }

    public decimal PrecioTotal { get; set; }
    
}