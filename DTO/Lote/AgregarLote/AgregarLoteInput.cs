namespace Costenita.DTO.Lote.AgregarLote;

public class AgregarLoteInput
{
    public required string Codigo { get; set; } 
    public DateTime FechaProduccion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int Cantidad { get; set; }

    public Guid ProductoId { get; set; }
}
