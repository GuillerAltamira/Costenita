using System;

namespace Costenita.Dto.Venta.ListarVenta;

public class DetalleVentaListarDTO
{
    public string Producto { get; set; } = "";
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
