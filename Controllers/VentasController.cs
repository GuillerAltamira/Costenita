using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;
using Costenita.DTO.Venta.ListarVenta;
using Costenita.DTO.Venta.AgregarVenta;
using Costenita.DTO.Venta.DetalleVenta;
using Costenita.DTO.Venta.ObtenerVenta;
using Costenita.DTO.Venta.EliminarVenta;

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
    public async Task<ActionResult<IEnumerable<ListarVentaOutput>>> GetVentas()
    {
        var ventas = await _contexto.Ventas
        .Include(v => v.Cliente)
        .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
        .Select(v => new ListarVentaOutput
        {
            Id = v.Id,
            Fecha = v.Fecha,
            Cliente = v.Cliente!.Nombre,
            FormaDePago = v.FormaDePago,
            Total = v.Total,
            Detalles = v.Detalles.Select(d => new DetalleVentaListarOutput
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
public async Task<ActionResult<ObtenerVentaOutput>> GetVentaById(Guid id)
{
    var venta = await _contexto.Ventas
        .Include(v => v.Cliente)
        .Include(v => v.Detalles)
            .ThenInclude(d => d.Producto)
        .FirstOrDefaultAsync(v => v.Id == id);

    if (venta == null)
        return NotFound("Venta no encontrada");

    var resultado = new ObtenerVentaOutput
    {
        Fecha = venta.Fecha,
        Cliente = venta.Cliente!.Nombre,
        FormaDePago = venta.FormaDePago,
        Total = venta.Total,
        Detalles = venta.Detalles.Select(d => new DetalleVentaOutput
        {
            Producto = d.Producto!.Nombre,
            Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario,
            Subtotal = d.Subtotal
        }).ToList()
    };

    return Ok(resultado);
}

    [HttpPost]
    public async Task<ActionResult> CreateVenta([FromBody] AgregarVentaInput dto)
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
             return BadRequest($"Producto no existe: {item.ProductoId}");

            int cantidadRestante = item.Cantidad;

            var lotes = await _contexto.Lotes
              .Where(l => l.ProductoId == item.ProductoId && l.Cantidad > 0)
              .OrderBy(l => l.FechaProduccion)
              .ToListAsync();

             if (!lotes.Any())
            return BadRequest($"No hay stock para {producto.Nombre}");

            foreach (var lote in lotes)
            {
              if (cantidadRestante <= 0)
              break;

              int descontar = Math.Min(lote.Cantidad, cantidadRestante);

              lote.Cantidad -= descontar;
              cantidadRestante -= descontar;
            }

            if (cantidadRestante > 0)
            return BadRequest($"Stock insuficiente en lotes para {producto.Nombre}");

            var detalle = new DetalleVenta
            {
                Id = Guid.NewGuid(),
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = producto.Precio * item.Cantidad
            };

            total += detalle.Subtotal;

            venta.Detalles.Add(detalle);
        }

        venta.Total = total;

       _contexto.Ventas.Add(venta);
       await _contexto.SaveChangesAsync();

       return Ok(new
      {
        venta.Id,
        venta.Fecha,
        venta.Total,
        Cliente = cliente.Nombre,
        Detalles = venta.Detalles.Select(d => new
        {
            Producto = _contexto.Productos
            .Where(p => p.Id == d.ProductoId)
            .Select(p => p.Nombre)
            .FirstOrDefault(),
            d.Cantidad,
            d.PrecioUnitario,
            d.Subtotal
        })
    });
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
    public async Task<ActionResult<EliminarVentaOutput>> DeleteVenta(Guid id)
    {
        var venta = await _contexto.Ventas
        .Include(v => v.Cliente)
        .Include(v => v.Detalles)
        .FirstOrDefaultAsync(v => v.Id == id);

        if (venta == null)
        return NotFound("Venta no encontrada");

       foreach (var detalle in venta.Detalles)
       {
        var lotes = await _contexto.Lotes
            .Where(l => l.ProductoId == detalle.ProductoId)
            .OrderByDescending(l => l.FechaProduccion)
            .ToListAsync();

          int cantidad = detalle.Cantidad;

          foreach (var lote in lotes)
          {
            lote.Cantidad += cantidad;
            break; 
          }
        }

        _contexto.Ventas.Remove(venta);
        await _contexto.SaveChangesAsync();

        return Ok(new EliminarVentaOutput
       {
          Mensaje = "Venta eliminada correctamente",
          Cliente = venta.Cliente!.Nombre,
          Total = venta.Total
       });
    }
}
