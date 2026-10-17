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

    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime FechaCompra { get; set; }

    [Required]
    [StringLength(100)]
    public string NombreCliente { get; set; }

    [Required]
    [StringLength(100)]
    public string ApellidosCliente { get; set; }

    [Required]
    [StringLength(200)]
    public string DireccionEnvio { get; set; }

    [Required]
    [StringLength(20)]
    public string NumeroTelefono { get; set; }

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    [Precision(10, 2)]
    public decimal PrecioTotal { get; set; }
    
    public IList<LineaCompraAccesorio> LineasCompra { get; set; } = new List<LineaCompraAccesorio>();
    
}