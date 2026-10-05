namespace AppForSEII.API.Models;

public class Modelo3D
{
    public Modelo3D(int id, string nombre, string categoría, FormatoModelo3D formato, decimal precio)
    {
        Id = id;
        Nombre = nombre;
        Categoría = categoría;
        Formato = formato;
        Precio = precio;
    }

    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Categoría { get; set; }
    public FormatoModelo3D Formato { get; set; }
    public decimal Precio { get; set; }
}