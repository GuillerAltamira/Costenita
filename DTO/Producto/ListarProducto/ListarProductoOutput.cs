using System;

namespace Costenita.Dto.Producto.ListarProducto;

public class ListarProductoOutput
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = "";
    public int Tamano { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}
