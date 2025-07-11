using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController2 : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ClientesController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
        {
            return await _context.Clientes
                .Select(c => new ClienteDTO
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Email = c.Email,
                    Telemovel = c.Telemovel,
                    Morada = c.Morada,
                    CodPostal = c.CodPostal
                }).ToListAsync();
        }

        // GET: api/ClientesController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            var c = await _context.Clientes.FindAsync(id);

            if (c == null) return NotFound();

            return new ClienteDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telemovel = c.Telemovel,
                Morada = c.Morada,
                CodPostal = c.CodPostal
            };
        }

        // POST: api/ClientesController2
        [HttpPost]
        public async Task<ActionResult> PostCliente([FromBody] ClienteDTO dto)
        {
            var cliente = new Clientes
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telemovel = dto.Telemovel,
                Morada = dto.Morada,
                CodPostal = dto.CodPostal,
                UserId = "" // definir se necessário
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        // PUT: api/ClientesController2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] ClienteDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            cliente.Nome = dto.Nome;
            cliente.Email = dto.Email;
            cliente.Telemovel = dto.Telemovel;
            cliente.Morada = dto.Morada;
            cliente.CodPostal = dto.CodPostal;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ClientesController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telemovel { get; set; }
        public string Email { get; set; }
        public string Morada { get; set; }
        public string CodPostal { get; set; }
    }
}
