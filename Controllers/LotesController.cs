using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;
using Costenita.DTO.Lote.AgregarLote;
using Costenita.DTO.Lote.ListarLotes;
using Costenita.DTO.Lote.ActualizarLote;
using Costenita.DTO.Lote.ObtenerLote;

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
    public async Task<ActionResult<IEnumerable<ListarLoteOutput>>> GetLotes()
   {
         var lotes = await _contexto.Lotes
        .Include(l => l.Producto)
        .Select(l => new ListarLoteOutput
        {
            Id = l.Id,
            Codigo = l.Codigo,
            FechaProduccion = l.FechaProduccion,
            FechaVencimiento = l.FechaVencimiento,
            Cantidad = l.Cantidad,
            ProductoNombre = l.Producto!.Nombre,
            ProductoPrecio = l.Producto.Precio
        })
        .ToListAsync();
        return Ok(lotes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ObtenerLoteOutput>> GetLote(Guid id)
    {
        var lote = await _contexto.Lotes
            .Include(l => l.Producto)
            .FirstOrDefaultAsync(l => l.Id == id);

            if (lote == null)
        return NotFound();

        return Ok(new ObtenerLoteOutput
        {
           Id = lote.Id,
           Codigo = lote.Codigo,
           FechaProduccion = lote.FechaProduccion,
           FechaVencimiento = lote.FechaVencimiento,
           Cantidad = lote.Cantidad,
           ProductoNombre = lote.Producto!.Nombre
        });
    }


    [HttpPost]
    public async Task<ActionResult> CreateLote([FromBody] AgregarLoteInput dto)
    {
        var producto = await _contexto.Productos.FindAsync(dto.ProductoId);

        if (producto == null)
        return BadRequest("Producto no existe");

        var lote = new Lote
        {
            Id = Guid.NewGuid(),
            ProductoId = dto.ProductoId,
            Cantidad = dto.Cantidad,
            FechaProduccion = dto.FechaProduccion,
            FechaVencimiento = dto.FechaVencimiento,
            Codigo = dto.Codigo
        };

        _contexto.Lotes.Add(lote);
        await _contexto.SaveChangesAsync();

        return Ok(new
        {
            Mensaje = "Lote agregado correctamente",
            Producto = producto.Nombre,
            Cantidad = lote.Cantidad,
            FechaProduccion = lote.FechaProduccion.ToShortDateString()
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLote(Guid id, [FromBody] ActualizarLoteInput dto)
    {
      var lote = await _contexto.Lotes.FindAsync(id);

      if (lote == null)
      return NotFound();

      var producto = await _contexto.Productos.FindAsync(lote.ProductoId);

       if (producto == null)
       return BadRequest("Producto no encontrado");

  
       int diferencia = dto.Cantidad - lote.Cantidad;

    
       producto.Stock += diferencia;

       if (producto.Stock < 0)
       return BadRequest("Stock no puede ser negativo");

  
       lote.Codigo = dto.Codigo;
       lote.FechaProduccion = dto.FechaProduccion;
       lote.FechaVencimiento = dto.FechaVencimiento;
       lote.Cantidad = dto.Cantidad;

       await _contexto.SaveChangesAsync();

       return NoContent(); 
    }
}
