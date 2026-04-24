using System;

namespace Costenita.DTO.Venta.ListarVenta;

public class ListarVentaOutput
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public required string Cliente { get; set; } 
    public required string FormaDePago { get; set; } 
    public decimal Total { get; set; }

    public List<DetalleVentaListarOutput> Detalles { get; set; } = new();
}
