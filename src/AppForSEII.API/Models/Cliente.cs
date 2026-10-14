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
    [Required,StringLength(100,ErrorMessage = "La dirección de facturación no puede superar los 100 caracteres")]
    public string DireccionFacturacion {get;set;}
}
    
