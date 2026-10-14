namespace AppForSEII.API.Models;

public class Cliente : ApplicationUser
{
    public Cliente()
    {
    }
    public Cliente(string id, string name, string surname, string userName, string direccionFacturacion) : 
    base(id, name, surname, userName)
    {
        DireccionFacturacion = direccionFacturacion;
    }
    [Required]
    [StringLength(200)]
    public string DireccionFacturacion {get;set;}
}
    
