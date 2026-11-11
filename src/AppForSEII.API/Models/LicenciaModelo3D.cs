namespace AppForSEII.API.Models;


public class LicenciaModelo3D

{

    public LicenciaModelo3D()
    {
        Nombre = string.Empty;
        Modelo3D = new Modelo3D();
    }

   

    public LicenciaModelo3D(string nombre, DateTime fechaExpiracion, Modelo3D modelo3D) : this()

    {

        Nombre = nombre;

        FechaExpiracion = fechaExpiracion;

        Modelo3DId = modelo3D.Id; 
        Modelo3D = modelo3D;

    }



    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    public DateTime FechaExpiracion { get; set; }

    //RELACIONES
    [Required]
    public int Modelo3DId { get; set; }
    
    public Modelo3D Modelo3D { get; set; }

}