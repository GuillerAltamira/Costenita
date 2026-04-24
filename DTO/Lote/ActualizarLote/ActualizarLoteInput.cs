namespace Costenita.DTO.Lote.ActualizarLote;

public class ActualizarLoteInput
{
    public required string Codigo { get; set; } 
    public DateTime FechaProduccion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int Cantidad { get; set; }
}