using System;

namespace Costenita.DTO.Cliente.ListarClientes;

public class ListarClientesOutput
{
    public Guid Id {get;set;}
    public string Nombre {get;set;} = string.Empty;
    public int Ci {get; set;}
}

