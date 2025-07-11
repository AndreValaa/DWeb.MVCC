using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController2 : ControllerBase
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
                return NotFound();

            return await _context.Compras.ToListAsync();
        }

        // GET: api/ComprasController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compras>> GetCompra(int id)
        {
            if (_context.Compras == null)
                return NotFound();

            var compra = await _context.Compras.FindAsync(id);

            if (compra == null)
                return NotFound();

            return compra;
        }

        // POST: api/ComprasController2
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras([FromBody] CompraDto compraDto)
        {
            if (_context.Compras == null)
                return Problem("Entity set 'ApplicationDbContext.Compras' is null.");

            var compra = new Compras
            {
                Email = compraDto.Email,
                ProdutosComprados = compraDto.ProdutosComprados,
                PrecoTotal = compraDto.PrecoTotal,
                QuantidadeTotal = compraDto.QuantidadeTotal,
                DataCompra = DateTime.Now
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompra), new { id = compra.Id }, compra);
        }

        // PUT: api/ComprasController2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompra(int id, [FromBody] CompraDto dto)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound();

            compra.Email = dto.Email;
            compra.ProdutosComprados = dto.ProdutosComprados;
            compra.PrecoTotal = dto.PrecoTotal;
            compra.QuantidadeTotal = dto.QuantidadeTotal;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ComprasController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompra(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound();

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class CompraDto
    {
        public string Email { get; set; }
        public string ProdutosComprados { get; set; }
        public decimal PrecoTotal { get; set; }
        public int QuantidadeTotal { get; set; }
    }
}
