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
    public class CategoriasController2 : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CategoriasController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            if (_context.Categorias == null)
                return NotFound();

            var categorias = await _context.Categorias
                .Include(c => c.Grupos)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    GruposId = c.GruposId,
                    NomeGrupo = c.Grupos.Nome
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // GET: api/CategoriasController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categorias>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Grupos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            return categoria;
        }

        // POST: api/CategoriasController2
        [HttpPost]
        public async Task<ActionResult<Categorias>> PostCategoria([FromBody] CategoriaDto dto)
        {
            var grupo = await _context.Grupos.FindAsync(dto.GruposId);
            if (grupo == null)
                return BadRequest("Grupo não encontrado.");

            var categoria = new Categorias
            {
                Nome = dto.Nome,
                GruposId = dto.GruposId
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        // PUT: api/CategoriasController2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, [FromBody] CategoriaDto dto)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return NotFound();

            categoria.Nome = dto.Nome;
            categoria.GruposId = dto.GruposId;

            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(categoria);
        }

        // DELETE: api/CategoriasController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int GruposId { get; set; }
        public string NomeGrupo { get; set; }
    }

}
