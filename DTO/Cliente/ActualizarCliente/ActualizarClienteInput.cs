using System.ComponentModel.DataAnnotations;

namespace Costenita.DTO.Cliente.ActualizarCliente;

public class ActualizarClienteInput
{
    [Required]
    public Guid Id { get; set; }

    [Range(1, int.MaxValue)]
    public int Ci { get; set; }

    [StringLength(2)]
    public string? Extension { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }
}
