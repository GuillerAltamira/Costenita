using System;

namespace Costenita.DTO.Venta.ListarVenta;

public class DetalleVentaListarOutput
{
    public string Producto { get; set; } = "";
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
