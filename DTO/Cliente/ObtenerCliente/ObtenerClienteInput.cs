namespace Costenita.DTO.Cliente.ObtenerCliente;

public class ObtenerClienteOutput
{
    public Guid Id { get; set; }
    public int Ci { get; set; }
    public string? Extension { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
}
