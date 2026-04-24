namespace Costenita.DTO.Venta.ObtenerVenta;
public class DetalleVentaOutput
{
    public required string Producto { get; set; } 
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}