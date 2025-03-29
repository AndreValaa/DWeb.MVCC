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
    // [ApiController]
    public class ProdutosController2 : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController2(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> Create([FromBody] Compras compras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compras);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // GET: api/ProdutosController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produtos>>> GetProdutos()
        {
          if (_context.Produtos == null)
          {
              return NotFound();
          }
            return await _context.Produtos.Include(p => p.Fotos).Include(p => p.Categoria).ToListAsync();
        }

        // GET: api/ProdutosController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produtos>> GetProdutos(int id)
        {
          if (_context.Produtos == null)
          {
              return NotFound();
          }
            var produtos = await _context.Produtos.FindAsync(id);

            if (produtos == null)
            {
                return NotFound();
            }

            return produtos;
        }

        // PUT: api/ProdutosController2/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdutos(int id, Produtos produtos)
        {
            if (id != produtos.Id)
            {
                return BadRequest();
            }

            _context.Entry(produtos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProdutosController2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produtos>> PostProdutos(Produtos produtos)
        {
          if (_context.Produtos == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Produtos'  is null.");
          }
            _context.Produtos.Add(produtos);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProdutos), new { id = produtos.Id }, produtos);
        }

        // DELETE: api/ProdutosController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdutos(int id)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }
            var produtos = await _context.Produtos.FindAsync(id);
            if (produtos == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produtos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutosExists(int id)
        {
            return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
