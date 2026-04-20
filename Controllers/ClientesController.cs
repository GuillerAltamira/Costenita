using Microsoft.AspNetCore.Mvc;
using Costenita.Dto.Cliente.AgregarCliente;
using Costenita.Dto.Cliente.ListarCliente;
using System;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;

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
public async Task<ActionResult<IEnumerable<ListarClienteDTO>>> GetClientes()
{
    var clientes = await _contexto.Clientes
        .Select(c => new ListarClienteDTO
        {
            Id = c.Id,
            Ci = c.Ci,
            Extension = c.Extension,
            Nombre = c.Nombre,
            FechaNacimiento = c.FechaNacimiento
        })
        .ToListAsync();

    return Ok(clientes);
}



    [HttpGet("{id}")]
public async Task<ActionResult<ListarClienteDTO>> GetCliente(Guid id)
{
    var cliente = await _contexto.Clientes
        .Where(c => c.Id == id)
        .Select(c => new ListarClienteDTO
        {
            Id = c.Id,
            Ci = c.Ci,
            Extension = c.Extension,
            Nombre = c.Nombre,
            FechaNacimiento = c.FechaNacimiento
        })
        .FirstOrDefaultAsync();

    if (cliente == null)
        return NotFound();

    return Ok(cliente);


}


    [HttpPost]
public async Task<ActionResult> CreateCliente(AgregarClienteDTO dto)
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

    return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
}



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCliente(Guid id, Cliente cliente)
    {
        if (id != cliente.Id)
            return BadRequest("ID no coincide");

        var existing = await _contexto.Clientes.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Nombre = cliente.Nombre;
        existing.Ci = cliente.Ci;
        existing.Extension = cliente.Extension;
        existing.FechaNacimiento = cliente.FechaNacimiento;

        await _contexto.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(Guid id)
    {
        var cliente = await _contexto.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        _contexto.Clientes.Remove(cliente);
        await _contexto.SaveChangesAsync();

        return NoContent();
    }
}

