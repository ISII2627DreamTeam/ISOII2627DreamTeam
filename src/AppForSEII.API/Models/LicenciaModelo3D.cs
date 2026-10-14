namespace AppForSEII.API.Models;


public class LicenciaModelo3D

{

    public LicenciaModelo3D() { }

   

    public LicenciaModelo3D(int id, string nombre, DateTime fechaExpiracion, int modelo3DId)

    {

        Id = id;

        Nombre = nombre;

        FechaExpiracion = fechaExpiracion;

        Modelo3DId = modelo3DId; 

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