using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;
using Costenita.Dto.Producto.AgregarProducto;
using Costenita.Dto.Producto.ListarProducto;

namespace Costenita.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _contexto;

    public ProductosController(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListarProductoOutput>>> GetProductos() 
    {
        var productos = await _contexto.Productos
        .Select(p => new ListarProductoOutput
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Tamano = p.Tamano,
            Precio = p.Precio,
            Stock = p.Lotes!.Sum(l => l.Cantidad)
        })
        .ToListAsync();

        return Ok(productos);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<ListarProductoOutput>> GetProducto(Guid id)
    {
         var producto = await _contexto.Productos
         .Where(p => p.Id == id)
         .Select(p => new ListarProductoOutput
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Precio = p.Precio,
            Stock = p.Lotes!.Sum(l => l.Cantidad)
        })
        .FirstOrDefaultAsync();

        if (producto == null)
        return NotFound();

        return Ok(producto);
    }



    [HttpPost]
    public async Task<ActionResult> CreateProducto([FromBody] AgregarProductoInput dto)
    {
        var producto = new Producto
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre,
            Tamano = dto.Tamano,
            Precio = dto.Precio
        };

        _contexto.Productos.Add(producto);
        await _contexto.SaveChangesAsync();

        return Ok(new
        {
            Mensaje = "Producto creado correctamente",
            Nombre = producto.Nombre,
            Tamano = producto.Tamano,
            Stock = 0 
        });
    }


    [HttpPut("{id}")]
     public async Task<ActionResult<ActualizarProductoOutput>> UpdateProducto(Guid id, [FromBody] ActualizarProductoInput dto)
    {
        var producto = await _contexto.Productos.FindAsync(id);

        if (producto == null)
        return NotFound("Producto no encontrado");

        producto.Nombre = dto.Nombre;
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        producto.Tamano = dto.Tamano;
        producto.Tipo = dto.Tipo;

        await _contexto.SaveChangesAsync();

        return Ok(new ActualizarProductoOutput
       {
            Mensaje = "Producto actualizado correctamente",
            NombreProducto = producto.Nombre
       });
    }
}
