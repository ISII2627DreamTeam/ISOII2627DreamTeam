using AppForSEII.Web.API;

namespace AppForSEII.Web
{
    //"Define un servicio". Una clase C# que guarda el carrito
    public class CompraAccesoriosStateContainer
    {
        // la compra (el carrito) se crea al crear el servicio
        public CompraAccesorioDTO Compra { get; private set; } = new CompraAccesorioDTO()
        {
            Lineas = new List<LineaCompraAccesorioDTO>()
        };

        // "Añádelo sin duplicados". Si el accesorio ya está, no se vuelve a añadir
        public void AddAccesorioToCompra(AccesorioDTO accesorio)
        {
            if (!Compra.Lineas.Any(l => l.NombreAccesorio == accesorio.Nombre))
                Compra.Lineas.Add(new LineaCompraAccesorioDTO()
                {
                    NombreAccesorio = accesorio.Nombre,
                    Cantidad = 1
                });
        }

        // "Elimina un item"
        public void RemoveLineaCompra(LineaCompraAccesorioDTO linea)
        {
            Compra.Lineas.Remove(linea);
        }

        // "Limpiar el carrito"
        public void ClearCarrito()
        {
            Compra.Lineas.Clear();
        }
    }
}