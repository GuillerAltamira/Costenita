using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;

namespace Costenita.Controllers
{
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
        public async Task<ActionResult<ICollection<Lote>>> GetLotes()
        {
            var lotes = await _contexto.Lotes
                .Include(l => l.Producto)
                .ToListAsync();

            return Ok(lotes);
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
        public async Task<ActionResult<Lote>> CreateLote([FromBody] Lote lote)
        {
            _contexto.Lotes.Add(lote);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLote), new { id = lote.Id }, lote);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLote(Guid id, [FromBody] Lote lote)
        {
            if (id != lote.Id)
                return BadRequest("El ID no coincide con el lote enviado.");

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
}
