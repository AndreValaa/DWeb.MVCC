using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TamanhosController2 : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TamanhosController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TamanhosController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tamanhos>>> GetTamanhos()
        {
            return await _context.Tamanhos.ToListAsync();
        }

        // GET: api/TamanhosController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tamanhos>> GetTamanho(int id)
        {
            var tamanho = await _context.Tamanhos.FindAsync(id);
            if (tamanho == null) return NotFound();
            return tamanho;
        }

        // POST: api/TamanhosController2
        [HttpPost]
        public async Task<ActionResult<Tamanhos>> PostTamanho([FromBody] TamanhoDto dto)
        {
            var novo = new Tamanhos { Nome = dto.Nome };
            _context.Tamanhos.Add(novo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTamanho), new { id = novo.Id }, novo);
        }

        // PUT: api/TamanhosController2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTamanho(int id, [FromBody] TamanhoDto dto)
        {
            var tamanho = await _context.Tamanhos.FindAsync(id);
            if (tamanho == null) return NotFound();

            tamanho.Nome = dto.Nome;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/TamanhosController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTamanho(int id)
        {
            var tamanho = await _context.Tamanhos.FindAsync(id);
            if (tamanho == null) return NotFound();

            _context.Tamanhos.Remove(tamanho);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class TamanhoDto
    {
        public string Nome { get; set; }
    }
}
