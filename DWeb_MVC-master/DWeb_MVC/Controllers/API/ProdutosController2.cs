using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
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
        /*
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
        */

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
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Cores)
                .Include(p => p.Tamanhos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produtos == null)
            {
                return NotFound();
            }

            return produtos;
        }
        /*
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
        */

        // PATCH: api/ProdutosController2/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProdutos(int id, [FromBody] ProdutoUpdateDTO dto)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Cores)
                .Include(p => p.Tamanhos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            if (dto.Nome != null) produto.Nome = dto.Nome;
            if (dto.Marca != null) produto.Marca = dto.Marca;
            if (dto.Preco.HasValue) produto.Preco = dto.Preco.Value;

            if (dto.CategoriaIds != null)
                produto.Categoria = await _context.Categorias.Where(c => dto.CategoriaIds.Contains(c.Id)).ToListAsync();

            if (dto.CorIds != null)
                produto.Cores = await _context.Cores.Where(c => dto.CorIds.Contains(c.Id)).ToListAsync();

            if (dto.TamanhoIds != null)
                produto.Tamanhos = await _context.Tamanhos.Where(t => dto.TamanhoIds.Contains(t.Id)).ToListAsync();

            await _context.SaveChangesAsync();

            return Ok();
        }


        // POST: api/ProdutosController2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produtos>> PostProdutos([FromBody] ProdutoDTO produtoDto)
        {
            if (_context.Produtos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Produtos'  is null.");
            }

            try
            {
                var produto = new Produtos
                {
                    Nome = produtoDto.Nome,
                    Marca = produtoDto.Marca,
                    Preco = produtoDto.Preco,
                    Categoria = _context.Categorias.Where(c => produtoDto.CategoriaIds.Contains(c.Id)).ToList(),
                    Cores = _context.Cores.Where(c => produtoDto.CorIds.Contains(c.Id)).ToList(),
                    Tamanhos = _context.Tamanhos.Where(t => produtoDto.TamanhoIds.Contains(t.Id)).ToList()
                };

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProdutos), new { id = produto.Id }, produto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
            }
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

public class ProdutoDTO
{
    public string Nome { get; set; }

    public string Marca { get; set; }

    public decimal Preco { get; set; }

    // IDs das categorias associadas
    public List<int> CategoriaIds { get; set; }

    // IDs das cores associadas
    public List<int> CorIds { get; set; }

    // IDs dos tamanhos associados
    public List<int> TamanhoIds { get; set; }
}

public class ProdutoUpdateDTO
{
    public string? Nome { get; set; }
    public string? Marca { get; set; }
    public decimal? Preco { get; set; }
    public List<int>? CategoriaIds { get; set; }
    public List<int>? CorIds { get; set; }
    public List<int>? TamanhoIds { get; set; }
}