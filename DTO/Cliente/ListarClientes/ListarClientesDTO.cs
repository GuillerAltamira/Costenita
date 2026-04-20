using System;

namespace Costenita.Dto.Cliente.ListarCliente;

public class ListarClienteDTO
{
    public Guid Id { get; set; }
    public int Ci { get; set; }
    public string? Extension { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime FechaNacimiento { get; set; }
}
