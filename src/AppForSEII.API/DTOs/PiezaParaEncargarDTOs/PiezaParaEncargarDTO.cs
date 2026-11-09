using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.EncargoImpresionDTOs
{
    // DTO de salida utilizado por el método GET de selección (GetPiezasParaEncargar).
    // Sirve para mostrar al usuario la lista de piezas disponibles en el catálogo con solo los datos necesarios
    // (evitando exponer toda la entidad Pieza3D de la base de datos).
    public class PiezaParaEncargarDTO
    {
        // Identificador único de la pieza 3D en la base de datos.
        public int Id { get; set; }

        // Nombre descriptivo de la pieza. Incluye validación de obligatoriedad y longitud máxima de 50 caracteres.
        [Required]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; }

        // Nombre de la categoría convertido a texto (string) a partir del Enum CategoriaPieza.
        [Required]
        public string Categoria { get; set; }

        // Peso en gramos de la pieza, necesario para calcular el consumo de material y el coste.
        [Required]
        public decimal Peso { get; set; }

        // Constructor parametrizado obligatorio según la rúbrica para proyectar (con .Select) 
        // cada entidad Pieza3D directamente a un PiezaParaEncargarDTO sin usar llaves { }.
        public PiezaParaEncargarDTO(int id, string nombre, string categoria, decimal peso)
        {
            Id = id;
            Nombre = nombre;
            Categoria = categoria;
            Peso = peso;
        }

        // Sobrescritura del método Equals obligatoria según la rúbrica (evita -1 punto de penalización).
        // Permite que Assert.Equal(expectedPiezas, piezasActualResult) compare dos listas de DTOs
        // comprobando si sus propiedades Id, Nombre, Categoria y Peso tienen el mismo valor.
        public override bool Equals(object? obj)
        {
            return obj is PiezaParaEncargarDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Categoria == dTO.Categoria &&
                   Peso == dTO.Peso;
        }

        // Genera el código hash a partir de las 4 propiedades del DTO.
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Categoria, Peso);
        }
    }
}