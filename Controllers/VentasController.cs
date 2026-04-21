using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;
using Costenita.Dto.Venta.AgregarVenta;
using Costenita.Dto.Venta.ListarVenta;

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
public async Task<ActionResult<IEnumerable<ListarVentaDTO>>> GetVentas()
{
    var ventas = await _contexto.Ventas
        .Include(v => v.Cliente)
        .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
        .Select(v => new ListarVentaDTO
        {
            Id = v.Id,
            Fecha = v.Fecha,
            Cliente = v.Cliente!.Nombre,
            FormaDePago = v.FormaDePago,
            Total = v.Total,
            Detalles = v.Detalles.Select(d => new DetalleVentaListarDTO
            {
                Producto = d.Producto!.Nombre,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList()
        })
        .ToListAsync();

    return Ok(ventas);
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
public async Task<ActionResult> CreateVenta(AgregarVentaDTO dto)
{
    var cliente = await _contexto.Clientes.FindAsync(dto.ClienteId);
    if (cliente == null)
        return BadRequest("Cliente no existe");

    var venta = new Venta
    {
        Id = Guid.NewGuid(),
        ClienteId = dto.ClienteId,
        FormaDePago = dto.FormaDePago,
        Fecha = DateTime.Now,
        Detalles = new List<DetalleVenta>()
    };

    decimal total = 0;

    foreach (var item in dto.Detalles)
    {
        var producto = await _contexto.Productos.FindAsync(item.ProductoId);

        if (producto == null)
            return BadRequest("Producto no existe");

        if (producto.Stock < item.Cantidad)
            return BadRequest($"Stock insuficiente para {producto.Nombre}");

        var detalle = new DetalleVenta
        {
            Id = Guid.NewGuid(),
            ProductoId = item.ProductoId,
            Cantidad = item.Cantidad,
            PrecioUnitario = producto.Precio,
            Subtotal = item.Cantidad * producto.Precio,
            VentaId = venta.Id
        };

        total += detalle.Subtotal;

        producto.Stock -= item.Cantidad;

        venta.Detalles.Add(detalle);
    }

    venta.Total = total;

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
