using System.ComponentModel.DataAnnotations;

namespace Costenita.Dto.Producto.AgregarProducto;

public class AgregarProductoInput
{
    [Required]
    [MaxLength(50)]
    public required string Nombre { get; set; } 
    [Required]
    public int Tamano { get; set; }
    public required string Tipo {get;set;} 

    [Required]
    public decimal Precio { get; set; }

    public int Stock { get; set; }

}
