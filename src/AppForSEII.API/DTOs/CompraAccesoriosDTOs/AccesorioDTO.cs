using System;
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class AccesorioDTO
    {
        // Atributo de validación (Data Annotation): El ModelState validará automáticamente 
        // que este string no tenga más de 100 caracteres antes de entrar al controlador.
        [StringLength(100)]
        public string Nombre { get; set; }
        
        public CategoriaAccesorio Categoria { get; set; }

        // =========================================================================
        // 1. CONSTRUCTOR DE MAPEO (Proyección)
        // =========================================================================
        // Este constructor es el que utilizas dentro del .Select(a => new AccesorioDTO(a)) 
        // en tu AccesoriosController. Coge la entidad pesada de la BD (Accesorio) 
        // y extrae únicamente el Nombre y la Categoría, dejando atrás los IDs u otros datos.

        public AccesorioDTO(Accesorio accesorio)
        {
            Nombre = accesorio.Nombre;
            Categoria = accesorio.Categoria;
        }

        // =========================================================================
        // 2. SOBRESCRITURA DE EQUALS (Obligatorio para los Tests)
        // =========================================================================
        // Por defecto, C# evalúa si dos objetos son el mismo comprobando si ocupan el 
        // mismo espacio en la memoria RAM. Si no sobrescribes esto, un Assert.Equal() 
        // en tus pruebas fallará aunque los dos DTOs tengan los mismos datos.
        public override bool Equals(object? obj)
        {
            // Comprueba si el objeto que nos pasan es realmente un AccesorioDTO (lo llama 'dTO').
            // Si lo es (&&), compara que sus valores internos sean matemáticamente iguales a los nuestros
            return obj is AccesorioDTO dTO &&
                   Nombre == dTO.Nombre &&
                   Categoria == dTO.Categoria;
        }

        // =========================================================================
        // 3. SOBRESCRITURA DE GETHASHCODE
        // =========================================================================
        // Es una regla de oro de C#: Si modificas Equals, debes modificar GetHashCode.
        // Combina los valores de Nombre y Categoría para generar un identificador numérico único.
        public override int GetHashCode()
        {
            return HashCode.Combine(Nombre, Categoria);
        }
    }
}