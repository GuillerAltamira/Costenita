namespace Costenita.DTO.Lote.ListarLotes;

public class ListarLoteOutput
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public DateTime FechaProduccion { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int Cantidad { get; set; }

    public string ProductoNombre { get; set; } = string.Empty;
    public decimal ProductoPrecio { get; set; }
}
