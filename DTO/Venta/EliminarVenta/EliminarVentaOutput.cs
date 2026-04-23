namespace Costenita.DTO.Venta.EliminarVenta;
public class EliminarVentaOutput
{
    public required string Cliente {get; set;}
    public string Mensaje { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
}