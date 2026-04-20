using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;

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
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        return Ok(await _contexto.Productos.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetProducto(Guid id)
    {
        var producto = await _contexto.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProducto(Producto producto)
    {
        if (producto.Precio <= 0)
            return BadRequest("Precio inválido");

        _contexto.Productos.Add(producto);
        await _contexto.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
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
