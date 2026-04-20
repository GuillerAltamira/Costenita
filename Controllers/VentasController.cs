using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;

namespace Costenita.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VentasController : ControllerBase
{
    private readonly AppDbContext _contexto;

    public VentasController(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
    {
        return Ok(await _contexto.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Venta>> GetVenta(Guid id)
    {
        var venta = await _contexto.Ventas
            .Include(v => v.Cliente)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venta == null)
            return NotFound();

        return Ok(venta);
    }

    [HttpPost]
    public async Task<ActionResult> CreateVenta(Venta venta)
    {
        var cliente = await _contexto.Clientes.FindAsync(venta.ClienteId);
        if (cliente == null)
            return BadRequest("Cliente no existe");

        decimal total = 0;

        foreach (var detalle in venta.Detalles)
        {
            var producto = await _contexto.Productos.FindAsync(detalle.ProductoId);

            if (producto == null)
                return BadRequest("Producto no existe");

            if (producto.Stock < detalle.Cantidad)
                return BadRequest($"Stock insuficiente para {producto.Nombre}");

            detalle.PrecioUnitario = producto.Precio;
            detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;

            total += detalle.Subtotal;

            // 🔥 DESCUENTO STOCK
            producto.Stock -= detalle.Cantidad;
        }

        venta.Total = total;
        venta.Fecha = DateTime.Now;

        _contexto.Ventas.Add(venta);
        await _contexto.SaveChangesAsync();

        return Ok(venta);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVenta(Guid id, Venta venta)
    {
        if (id != venta.Id)
            return BadRequest();

        var existing = await _contexto.Ventas.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.FormaDePago = venta.FormaDePago;

        await _contexto.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenta(Guid id)
    {
        var venta = await _contexto.Ventas.FindAsync(id);

        if (venta == null)
            return NotFound();

        _contexto.Ventas.Remove(venta);
        await _contexto.SaveChangesAsync();

        return NoContent();
    }
}
