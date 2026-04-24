namespace Costenita.DTO.Lote.ListarLotes;

public class ListarLoteOutput
{
    public Guid Id { get; set; }
    public required string Codigo { get; set; } 
    public DateTime FechaProduccion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int Cantidad { get; set; }

    public required string ProductoNombre { get; set; } 
    public decimal ProductoPrecio { get; set; }
}
