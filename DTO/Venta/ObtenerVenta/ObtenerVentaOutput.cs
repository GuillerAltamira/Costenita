namespace Costenita.DTO.Venta.ObtenerVenta;
public class ObtenerVentaOutput
{
    public DateTime Fecha { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string FormaDePago { get; set; } = string.Empty;
    public decimal Total { get; set; }

    public List<DetalleVentaOutput> Detalles { get; set; } = new();
}