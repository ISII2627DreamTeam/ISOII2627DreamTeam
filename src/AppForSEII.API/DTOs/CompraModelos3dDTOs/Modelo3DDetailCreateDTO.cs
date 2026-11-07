using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AppForSEII.API.Models; // necesario para que reconozca el enum FormatoModelo3D

namespace AppForSEII.API.DTOs.CompraModelos3DDTOs
{
    public class Modelo3DDetailDTO
    {
        // el id lo dejo tal cual
        // en el post vendrá vacío y la base de datos lo autogenera, pero en el get me trae el real.
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del modelo es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(50, ErrorMessage = "La categoría no puede superar los 50 caracteres")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "El formato es obligatorio")]
        public FormatoModelo3D Formato { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debe incluir la lista de licencias")]
        public IList<LicenciaModelo3DDTO> Licencias { get; set; }

        // constructor vacío obligatorio para que el post no falle
        public Modelo3DDetailDTO()
        {
            Nombre = string.Empty;
            Categoria = string.Empty;
            // inicializo la lista de licencias aquí para que no de error de null.
            Licencias = new List<LicenciaModelo3DDTO>();
        }

        // constructor con los parámetros sueltos. 
        // es el que llamo en el get details dentro del .Select() 
        public Modelo3DDetailDTO(int id, string nombre, string categoria, FormatoModelo3D formato, decimal precio, IList<LicenciaModelo3DDTO> licencias)
        {
            Id = id;
            Nombre = nombre;
            Categoria = categoria;
            Formato = formato;
            Precio = precio;
            Licencias = licencias;
        }

        public override bool Equals(object? obj)
        {
            // en el equals, para comparar las listas tengo que usar SequenceEqual sí o sí, o los asserts de los tests van a fallar.
            return obj is Modelo3DDetailDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Categoria == dTO.Categoria &&
                   Formato == dTO.Formato &&
                   Precio == dTO.Precio &&
                   Licencias.SequenceEqual(dTO.Licencias);
        }

        public override int GetHashCode()
        {
            // si tengo que meter un campo nuevo al DTO, lo tengo que añadir al Equals y también dentro de este paréntesis.
            return HashCode.Combine(Id, Nombre, Categoria, Formato, Precio, Licencias);
        }
    }
}