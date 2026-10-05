namespace AppForSEII.API.Models;



public class Pieza3D
{
    public Pieza3D() 
    {
        
    }

    public Pieza3D(int id, string nombre, decimal peso, CategoriaPieza categoria)  
    {
        Id = id;
        Nombre = nombre;
        Peso = peso;
        Categoria = categoria;
        
    }

    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Peso { get; set; }
    public CategoriaPieza Categoria { get; set; }
   
}