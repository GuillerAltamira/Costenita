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
public async Task<ActionResult<IEnumerable<ListarProductoDTO>>> GetProductos()
{
    var productos = await _contexto.Productos
        .Select(p => new ListarProductoDTO
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
public async Task<ActionResult<ListarProductoDTO>> GetProducto(Guid id)
{
    var producto = await _contexto.Productos
        .Where(p => p.Id == id)
        .Select(p => new ListarProductoDTO
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
public async Task<ActionResult> CreateProducto(AgregarProductoDTO dto)
{
    if (dto.Precio <= 0)
        return BadRequest("Precio inválido");

    var producto = new Producto
    {
        Id = Guid.NewGuid(),
        Nombre = dto.Nombre,
        Tamano = dto.Tamano,
        Precio = dto.Precio,
        Stock = dto.Stock
    };

    _contexto.Productos.Add(producto);
    await _contexto.SaveChangesAsync();

    return Ok(producto);
}


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProducto(Guid id, Producto producto)
    {
        if (id != producto.Id)
            return BadRequest("ID no coincide");

        var existing = await _contexto.Productos.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Nombre = producto.Nombre;
        existing.Precio = producto.Precio;
        existing.Stock = producto.Stock;

        await _contexto.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(Guid id)
    {
        var producto = await _contexto.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        _contexto.Productos.Remove(producto);
        await _contexto.SaveChangesAsync();

        return NoContent();
    }
}
