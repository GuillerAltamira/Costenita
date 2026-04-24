namespace Costenita.DTO.Venta.ObtenerVenta;
public class ObtenerVentaOutput
{
    public DateTime Fecha { get; set; }
    public required string Cliente { get; set; } 
    public required string FormaDePago { get; set; } 
    public decimal Total { get; set; }

    public List<DetalleVentaOutput> Detalles { get; set; } = new();
}