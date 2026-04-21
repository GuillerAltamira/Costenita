using System.ComponentModel.DataAnnotations;

namespace Costenita.Dto.Producto.AgregarProducto;

public class AgregarProductoDTO
{
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = "";
    [Required]
    public int Tamano { get; set; }
    public string Tipo {get;set;} = "";

    [Required]
    public decimal Precio { get; set; }

    public int Stock { get; set; }

}
