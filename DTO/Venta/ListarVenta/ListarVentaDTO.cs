using System;

namespace Costenita.Dto.Venta.ListarVenta;

public class ListarVentaDTO
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Cliente { get; set; } = "";
    public string FormaDePago { get; set; } = "";
    public decimal Total { get; set; }

    public List<DetalleVentaListarDTO> Detalles { get; set; } = new();
}
