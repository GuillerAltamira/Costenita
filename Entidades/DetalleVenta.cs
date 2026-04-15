using System;

namespace Costenita.Entidades;

public class DetalleVenta
{
    public Guid Id { get; set; }

    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }

    public Guid VentaId { get; set; }
    public Venta? Venta { get; set; }

    public Guid ProductoId { get; set; }
    public Producto? Producto { get; set; }
}
