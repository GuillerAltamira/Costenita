using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Costenita.Data;
using Costenita.Entidades;

namespace Costenita.Controllers
{
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
        public async Task<ActionResult<ICollection<Cliente>>> GetClientes()
        {
            var clientes = await _contexto.Clientes.ToListAsync();
            return Ok(clientes);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(Guid id)
        {
            var cliente = await _contexto.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        
        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente([FromBody] Cliente cliente)
        {
            _contexto.Clientes.Add(cliente);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(Guid id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest("El ID no coincide con el cliente enviado.");

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
}

