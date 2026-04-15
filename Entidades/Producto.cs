using System;
using System.Collections.Generic;

namespace Costenita.Entidades;

public class Producto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = "";
    public string? Tipo { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }

    public List<Lote>? Lotes { get; set; }
}
