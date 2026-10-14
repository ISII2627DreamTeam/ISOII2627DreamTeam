namespace AppForSEII.API.Models;

public class Impresora3D
{public Impresora3D(string nombre, TipoImpresora tipo,String modelo, string descripcion, decimal precioKilovatioHora, decimal precioReserva )
    {
        Nombre = nombre;
        Tipo = tipo;
        Modelo = modelo;
        Descripcion = descripcion;
        PrecioKilovatioHora = precioKilovatioHora;
        PrecioReserva = precioReserva;
    }
    public int Id { get; set; }

    [Key]
    public string Nombre { get; set; }

    public TipoImpresora Tipo { get; set; }
    [Required, MaxLength(100)]
    public string Modelo { get; set; }
    [Required, StringLength(500,ErrorMessage="La descripción no puede superar los 500 caracteres.")]
    public string Descripcion { get; set; }
    [Precision(10,2)]
    public decimal PrecioKilovatioHora { get; set; }
    [Precision(10,2)]
    public decimal PrecioReserva { get; set; }

    [Required]
    public IList<LineaReserva> LineasReserva { get; set; }
}

