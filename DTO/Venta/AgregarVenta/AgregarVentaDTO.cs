using System;
using System.ComponentModel.DataAnnotations;

namespace Costenita.Dto.Venta.AgregarVenta;

public class AgregarVentaDTO
{
    [Required]
    public Guid ClienteId { get; set; }

    [Required]
    public string FormaDePago { get; set; } = "";

    public List<DetalleVentaDTO> Detalles { get; set; } = new();
}
