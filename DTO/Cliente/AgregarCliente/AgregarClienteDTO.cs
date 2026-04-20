using System.ComponentModel.DataAnnotations;

namespace Costenita.Dto.Cliente.AgregarCliente;

public class AgregarClienteDTO
{
    [Required]
    public int Ci { get; set; }

    [MaxLength(2)]
    public string? Extension { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = "";

    public DateTime FechaNacimiento { get; set; }
}
