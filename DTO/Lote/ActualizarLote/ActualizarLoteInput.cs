namespace Costenita.DTO.Lote.ActualizarLote;

public class ActualizarLoteInput
{
    public string Codigo { get; set; } = string.Empty;
    public DateTime FechaProduccion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int Cantidad { get; set; }
}