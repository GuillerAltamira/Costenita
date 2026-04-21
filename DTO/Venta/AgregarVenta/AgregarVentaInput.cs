using Costenita.DTO.Venta.DetalleVenta;

namespace Costenita.DTO.Venta.AgregarVenta;

public class AgregarVentaInput
{
    public Guid ClienteId { get; set; }
    public string FormaDePago { get; set; } = string.Empty;

    public List<DetalleVentaInput> Detalles { get; set; } = new();
}
