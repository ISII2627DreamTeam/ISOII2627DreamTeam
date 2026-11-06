using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.CompraAccesoriosDTOs
{
    public class LineaCompraAccesorioForCreateDTO
    {
        [Required]
        public int AccesorioId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        public LineaCompraAccesorioForCreateDTO()
        {
            AccesorioId = 0;
            Cantidad = 0;
        }
        public LineaCompraAccesorioForCreateDTO(int accesorioId, int cantidad)
        {
            AccesorioId = accesorioId;
            Cantidad = cantidad;
        }
    }
}