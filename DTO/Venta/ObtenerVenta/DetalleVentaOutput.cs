namespace Costenita.DTO.Venta.ObtenerVenta;
public class DetalleVentaOutput
{
    public string Producto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}