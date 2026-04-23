namespace Costenita.DTO.Lote.ObtenerLote;

public class ObtenerLoteOutput
{
    public Guid Id { get; set; }
    public required string Codigo { get; set; }
    public DateTime FechaProduccion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int Cantidad { get; set; }

    public required string ProductoNombre { get; set; }
}
