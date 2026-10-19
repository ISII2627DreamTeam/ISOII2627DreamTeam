namespace AppForSEII.API.Models;

public class Modelo3D
{
    public Modelo3D() { }

    public Modelo3D(int id, string nombre, string categoría, FormatoModelo3D formato, decimal precio)
    {
        Id = id;
        Nombre = nombre;
        Categoría = categoría;
        Formato = formato;
        Precio = precio;
    }

    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(100)]
    public string Categoría { get; set; }

    [Required]
    public FormatoModelo3D Formato { get; set; }

    [Required]
    public decimal Precio { get; set; }

    public IList<LineaCompraModelo> LineasCompra { get; set; } = new List<LineaCompraModelo>();

    //(1 Modelo3D tiene de 0 a muchas Licencias)
    public IList<LicenciaModelo3D> Licencias { get; set; } = new List<LicenciaModelo3D>();
}