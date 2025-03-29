using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers
{
    [Route("api/[controller]")]
    public class ComprasController2 : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ComprasController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compras>>> GetCompras()
        {
            if (_context.Compras == null)
            {
                return NotFound();
            }
            return await _context.Compras.Include(c => c.Cliente).ToListAsync();
        }

        // POST: api/ComprasController2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras([FromBody] CompraDto compraDto)
        {
            if (_context.Compras == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Compras' is null.");
            }

            // Log dos dados recebidos
            Console.WriteLine($"Received Pago: {compraDto.Pago}, ClientesFK: {compraDto.ClientesFK}");

            var cliente = await _context.Clientes.FindAsync(compraDto.ClientesFK);
            if (cliente == null)
            {
                return BadRequest("Cliente não encontrado");
            }

            var c = new Compras
            {
                Pago = compraDto.Pago,
                ClientesFK = compraDto.ClientesFK
            };

            _context.Compras.Add(c);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompras), new { id = c.Id }, c);
        }
    }
    public class CompraDto
    {
        public bool Pago { get; set; }
        public int ClientesFK { get; set; }
    }
}
