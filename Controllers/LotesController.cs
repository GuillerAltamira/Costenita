using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;

namespace Costenita.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LotesController : ControllerBase
{
    private readonly AppDbContext _contexto;

    public LotesController(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lote>>> GetLotes()
    {
        return Ok(await _contexto.Lotes
            .Include(l => l.Producto)
            .ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Lote>> GetLote(Guid id)
    {
        var lote = await _contexto.Lotes
            .Include(l => l.Producto)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lote == null)
            return NotFound();

        return Ok(lote);
    }

    [HttpPost]
    public async Task<ActionResult<Lote>> CreateLote(Lote lote)
    {
        var producto = await _contexto.Productos.FindAsync(lote.ProductoId);

        if (producto == null)
            return BadRequest("Producto no existe");

        // 🔥 SUMAR STOCK
        producto.Stock += lote.Cantidad;

        _contexto.Lotes.Add(lote);
        await _contexto.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLote), new { id = lote.Id }, lote);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLote(Guid id, Lote lote)
    {
        if (id != lote.Id)
            return BadRequest("ID no coincide");

        var existing = await _contexto.Lotes.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Codigo = lote.Codigo;
        existing.FechaProduccion = lote.FechaProduccion;
        existing.FechaVencimiento = lote.FechaVencimiento;
        existing.Cantidad = lote.Cantidad;
        existing.ProductoId = lote.ProductoId;

        await _contexto.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLote(Guid id)
    {
        var lote = await _contexto.Lotes.FindAsync(id);
        if (lote == null)
            return NotFound();

        _contexto.Lotes.Remove(lote);
        await _contexto.SaveChangesAsync();
        return NoContent();
    }
}
