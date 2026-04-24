namespace Costenita.DTO.Venta.EliminarVenta;
public class EliminarVentaOutput
{
    public required string Cliente {get; set;}
    public required string Mensaje { get; set; } 
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
}