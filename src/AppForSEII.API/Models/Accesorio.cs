namespace AppForSEII.API.Models;

using System.Collections.Generic;
public class Accesorio
{
    public Accesorio()
    {
    }

    public Accesorio(int id, string nombre, CategoriaAccesorio categoria, string compatibilidad, 
        int cantidadDisponible, decimal precio)
    {
        Id = id;
        Nombre = nombre;
        Categoria = categoria;
        Compatibilidad = compatibilidad;
        CantidadDisponible = cantidadDisponible;
        Precio = precio;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    public CategoriaAccesorio Categoria { get; set; }

    [Required]
    [StringLength(100)]
    public string Compatibilidad { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int CantidadDisponible { get; set; }

    [Required]
    [Precision(8, 2)]
    public decimal Precio { get; set; }

    public IList<LineaCompraAccesorio> LineasCompra { get; set; } = new List<LineaCompraAccesorio>();
}