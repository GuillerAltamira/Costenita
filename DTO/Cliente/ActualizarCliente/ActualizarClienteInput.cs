namespace Costenita.DTO.Cliente.ActualizarCliente;
public class ActualizarClienteInput
{
    public required int Ci { get; set; } 
    public required string Extension { get; set; } 
    public required string Nombre { get; set; } 
    public DateTime FechaNacimiento { get; set; }
}