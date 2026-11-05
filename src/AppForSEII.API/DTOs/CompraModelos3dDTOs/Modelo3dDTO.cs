using System;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraModelos3DDTOs
{
    public class Modelo3DDTO
    {
        // Atributos base para la Historia de Usuario.
        //Si tengo que mostrar más datos del modelo (como el Id), añadirlos justo debajo.
        [StringLength(100)]
        public string Nombre { get; set; }
        
        [StringLength(100)]
        public string Categoría { get; set; }
        
        public FormatoModelo3D Formato { get; set; } 
        
        public decimal Precio { get; set; }

        // Constructor vacío para que las llamadas de la API lo puedan instanciar.
        // Si añado más strings arriba, inicializarlos aquí a string.Empty para que no salte warning al compilar.
        public Modelo3DDTO()
        {
            Nombre = string.Empty;
            Categoría = string.Empty;
        }

        // Constructor de mapeo desde la entidad principal.
        // Si añado nuevas propiedades arriba, tengo que acordarme de igualarlas aquí (ej: NuevaProp = modelo.NuevaProp;)
        public Modelo3DDTO(Modelo3D modelo) : this()
        {
            Nombre = modelo.Nombre;
            Categoría = modelo.Categoría;
            Formato = modelo.Formato;
            Precio = modelo.Precio;
        }

        // Sobrescritura del Equals para que pasen los Asserts de las pruebas unitarias.
        // IMPORTANTE: Si añado campos nuevos al DTO, hay que añadirlos obligatoriamente a esta lista de comparaciones con "&&".
        public override bool Equals(object? obj)
        {
            return obj is Modelo3DDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Categoría == dTO.Categoría &&
                   Formato == dTO.Formato &&
                   Precio == dTO.Precio;
        }

        // HashCode combinado. 
        // Igual que el Equals: si meto campos nuevos arriba, meterlos también dentro de este paréntesis.
        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Categoría, Formato, Precio);
        }
    }
}