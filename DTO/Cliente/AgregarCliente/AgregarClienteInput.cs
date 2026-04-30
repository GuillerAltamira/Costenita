using System.ComponentModel.DataAnnotations;

namespace Costenita.DTO.Cliente.AgregarCliente;

public class AgregarClienteInput
{
    [Required]
    public int Ci { get; set; }

    [MaxLength(5)]
    public required string Extension { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = "";

    public DateTime FechaNacimiento { get; set; }
}
