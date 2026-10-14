namespace AppForSEII.API.Models;
using System.ComponentModel.DataAnnotations;
public class ReservaImpresora
{
    public ReservaImpresora()
    {
        FechaReserva = DateTime.Now;
        NombreCliente = string.Empty;
        ApellidosCliente = string.Empty;
        DireccionFacturacion = string.Empty;
        PrecioTotal = 0;
        MetodoPago = MetodoPago.TarjetaCredito;
        LineasReserva = new List<LineaReserva>();
    }

    public ReservaImpresora( DateTime fechaReserva, string nombreCliente, string apellidosCliente, string direccionFacturacion, decimal precioTotal, MetodoPago metodoPago)
    :this()
    {
    
        FechaReserva = fechaReserva;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionFacturacion = direccionFacturacion;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
    }
    [Key]
    public int Id { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime FechaReserva { get; set; }
    [Required,StringLength(50,ErrorMessage = "El nombre del cliente no puede superar los 50 caracteres")]
    public string NombreCliente { get; set; }
    [StringLength(100,ErrorMessage = "Los apellidos del cliente no pueden superar los 100 caracteres")]
    public string ApellidosCliente { get; set; }
    [StringLength(200,ErrorMessage = "La dirección de facturación no puede superar los 200 caracteres")]
    public string DireccionFacturacion { get; set; }
    //la propiedad PrecioTotal > 0, se calcula a partir de las lineas de reserva, por lo que no es necesario validarla
    [Precision(10,2), Range(0, double.MaxValue, ErrorMessage = "El precio total debe ser mayor o igual a 0")]
    public decimal PrecioTotal { get; set; }
    
    public MetodoPago MetodoPago { get; set; }

    public IList<LineaReserva> LineasReserva { get; set; }
}