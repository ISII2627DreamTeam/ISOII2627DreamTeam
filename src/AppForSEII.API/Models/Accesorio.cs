namespace AppForSEII.API.Models;

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

    public int Id { get; set; }

    public string Nombre { get; set; }

    public CategoriaAccesorio Categoria { get; set; }

    public string Compatibilidad { get; set; }

    public int CantidadDisponible { get; set; }

    public decimal Precio { get; set; }
}