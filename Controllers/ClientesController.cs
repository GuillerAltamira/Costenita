using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;
using Costenita.DTO.Cliente.AgregarCliente;
using Costenita.DTO.Cliente.ListarCliente;
using Costenita.DTO.Cliente.ActualizarCliente;
using Costenita.DTO.Cliente.ObtenerCliente;

namespace Costenita.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _contexto;

    public ClientesController(AppDbContext contexto)
    {
        _contexto = contexto;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListarClienteOutput>>> GetClientes()
    {
        var clientes = await _contexto.Clientes
            .Select(c => new ListarClienteOutput
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Extension = c.Extension,
                Ci = c.Ci,
                FechaNacimiento = c.FechaNacimiento
            })
            .ToListAsync();

        return Ok(clientes);
    }
    [HttpGet("buscar")]
    public async Task<ActionResult> BuscarClientes([FromQuery] string? nombre)
    {
        var query = _contexto.Clientes.AsQueryable();

        if (!string.IsNullOrEmpty(nombre))
        {
            query = query.Where(c => c.Nombre.Contains(nombre));
        }

        var clientes = await query
        .Select(c => new ListarClienteOutput
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Ci = c.Ci,
            Extension = c.Extension,
            FechaNacimiento = c.FechaNacimiento

        })
        .ToListAsync();

         return Ok(clientes);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<ObtenerClienteOutput>> GetCliente(Guid id)
    {
        var cliente = await _contexto.Clientes
            .Where(c => c.Id == id)
            .Select(c => new ObtenerClienteOutput
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Ci = c.Ci,
                Extension = c.Extension,
                FechaNacimiento = c.FechaNacimiento
            })
            .FirstOrDefaultAsync();

        if (cliente == null)
            return NotFound("Cliente no encontrado");

        return Ok(cliente);
    }


    [HttpPost]
    public async Task<ActionResult> CreateCliente([FromBody] AgregarClienteInput dto)
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Ci = dto.Ci,
            Extension = dto.Extension,
            Nombre = dto.Nombre,
            FechaNacimiento = dto.FechaNacimiento
        };

        _contexto.Clientes.Add(cliente);
        await _contexto.SaveChangesAsync();

        Console.WriteLine(dto.Extension);

        return Ok(new
        {
            mensaje = "Cliente creado correctamente",
            cliente.Nombre,
            cliente.Ci
        });
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCliente(Guid id, [FromBody] ActualizarClienteInput dto)
    {
        var cliente = await _contexto.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound("Cliente no encontrado");

        cliente.Ci = dto.Ci;
        cliente.Extension = dto.Extension;
        cliente.Nombre = dto.Nombre;
        cliente.FechaNacimiento = dto.FechaNacimiento;

        await _contexto.SaveChangesAsync();

        return Ok(new
        {
            mensaje = "Cliente actualizado correctamente",
            cliente.Nombre
        });
    }
}