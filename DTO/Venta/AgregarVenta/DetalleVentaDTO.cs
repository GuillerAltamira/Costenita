using System;

namespace Costenita.Dto.Venta.AgregarVenta;

public class DetalleVentaDTO
{
    public Guid ProductoId { get; set; }
    public int Cantidad { get; set; }
}
