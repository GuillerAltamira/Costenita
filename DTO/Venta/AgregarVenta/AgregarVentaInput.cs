using Costenita.DTO.Venta.DetalleVenta;

namespace Costenita.DTO.Venta.AgregarVenta;

public class AgregarVentaInput
{
    public Guid ClienteId { get; set; }
    public required string FormaDePago { get; set; }

    public List<DetalleVentaInput> Detalles { get; set; } = new();
}
