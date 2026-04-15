using System;

namespace Costenita.Entidades;

public class Lote
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = "";
    public DateTime FechaProduccion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int Cantidad { get; set; }

    public Guid ProductoId { get; set; }
    public Producto? Producto { get; set; }
}
