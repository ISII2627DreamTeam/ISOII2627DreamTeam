namespace APPForSEII.API.Models;


public class CompraModelo3D
{

    public CompraModelo3D(int id, DateTime fechaCompra, string nombreCliente, string apellidosCliente, string correoElectronico, string direccionFacturacion, string? descripcion, decimal precioTotal, MetodoPago metodoPago)
{
    Id = id;
    FechaCompra = fechaCompra;
    NombreCliente = nombreCliente;
    ApellidosCliente = apellidosCliente;
    CorreoElectronico = correoElectronico;
    DireccionFacturacion = direccionFacturacion;
    Descripcion = descripcion;
    PrecioTotal = precioTotal;
    MetodoPago = metodoPago;
}

    public int Id { get; set; }
    public DateTime FechaCompra { get; set; }
    public string NombreCliente { get; set; }
    public string ApellidosCliente { get; set; }
    public string CorreoElectronico { get; set; }
    public string DireccionFacturacion { get; set; }
    public string? Descripcion { get; set; }
    public decimal PrecioTotal { get; set; }

    public MetodoPago MetodoPago { get; set; }
}