using System;

namespace Costenita.Entidades;

public class Venta
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string FormaDePago { get; set; } = "";

    public Guid ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public List<DetalleVenta>? Detalles {get; set;}
}

