using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController2 : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProdutosController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> GetProdutos()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Fotos)
                .Include(p => p.Categoria)
                .Include(p => p.Cores)
                .Include(p => p.Tamanhos)
                .ToListAsync();

            var resultado = produtos.Select(p => new ProdutoResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Marca = p.Marca,
                Preco = p.Preco,
                CategoriaNomes = p.Categoria.Select(c => c.Nome).ToList(),
                Cores = p.Cores.Select(c => c.Nome).ToList(),
                Tamanhos = p.Tamanhos.Select(t => t.Nome).ToList(),
                Fotos = p.Fotos.Select(f => f.NomeFicheiro).ToList()
            });

            return Ok(resultado);
        }

        // GET: api/ProdutosController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> GetProduto(int id)
        {
            var p = await _context.Produtos
                .Include(p => p.Fotos)
                .Include(p => p.Categoria)
                .Include(p => p.Cores)
                .Include(p => p.Tamanhos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null)
                return NotFound();

            return new ProdutoResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Marca = p.Marca,
                Preco = p.Preco,
                CategoriaNomes = p.Categoria.Select(c => c.Nome).ToList(),
                Cores = p.Cores.Select(c => c.Nome).ToList(),
                Tamanhos = p.Tamanhos.Select(t => t.Nome).ToList(),
                Fotos = p.Fotos.Select(f => f.NomeFicheiro).ToList()
            };
        }

        // POST: api/ProdutosController2
        [HttpPost]
        public async Task<ActionResult> PostProduto([FromBody] ProdutoDTO produtoDto)
        {
            var produto = new Produtos
            {
                Nome = produtoDto.Nome,
                Marca = produtoDto.Marca,
                Preco = produtoDto.Preco,
                Categoria = await _context.Categorias.Where(c => produtoDto.CategoriaIds.Contains(c.Id)).ToListAsync(),
                Cores = await _context.Cores.Where(c => produtoDto.CorIds.Contains(c.Id)).ToListAsync(),
                Tamanhos = await _context.Tamanhos.Where(t => produtoDto.TamanhoIds.Contains(t.Id)).ToListAsync()
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // PUT: api/ProdutosController2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, [FromBody] ProdutoDTO dto)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Cores)
                .Include(p => p.Tamanhos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            produto.Nome = dto.Nome;
            produto.Marca = dto.Marca;
            produto.Preco = dto.Preco;
            produto.Categoria = await _context.Categorias.Where(c => dto.CategoriaIds.Contains(c.Id)).ToListAsync();
            produto.Cores = await _context.Cores.Where(c => dto.CorIds.Contains(c.Id)).ToListAsync();
            produto.Tamanhos = await _context.Tamanhos.Where(t => dto.TamanhoIds.Contains(t.Id)).ToListAsync();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ProdutosController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
                return NotFound();

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class ProdutoDTO
    {
        public string Nome { get; set; }
        public string Marca { get; set; }
        public decimal Preco { get; set; }
        public List<int> CategoriaIds { get; set; }
        public List<int> CorIds { get; set; }
        public List<int> TamanhoIds { get; set; }
    }

    public class ProdutoResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public decimal Preco { get; set; }
        public List<string> CategoriaNomes { get; set; }
        public List<string> Cores { get; set; }
        public List<string> Tamanhos { get; set; }
        public List<string> Fotos { get; set; }
    }
}
