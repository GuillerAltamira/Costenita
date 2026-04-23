using System;

namespace Costenita.DTO.Cliente.ListarCliente;

public class ListarClienteOutput
{
    public Guid Id { get; set; }
    public int Ci { get; set; }
    public string? Extension { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime FechaNacimiento { get; set; }
}
