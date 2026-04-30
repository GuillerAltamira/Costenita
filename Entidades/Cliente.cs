using System;
using System.ComponentModel.DataAnnotations;

namespace Costenita.Entidades;

public class Cliente
{
    public Guid Id { get; set; }
    public int Ci { get; set; }

    [MaxLength(10)]
    public required string Extension { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime FechaNacimiento { get; set; }
}
