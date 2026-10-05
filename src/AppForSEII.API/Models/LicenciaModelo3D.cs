namespace AppForSEII.API.Models;



public class LicenciaModelo3D

{

    public LicenciaModelo3D() { }

   

    public LicenciaModelo3D(int id, string nombre, DateTime fechaExpiracion)

    {

        Id = id;

        Nombre = nombre;

        FechaExpiracion = fechaExpiracion;

    }



    [Key]

    public int Id { get; set; }



    [Required]

    public string Nombre { get; set; }



    [Required]

    public DateTime FechaExpiracion { get; set; }

}