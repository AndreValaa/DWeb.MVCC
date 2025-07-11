using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoresController2 : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoresController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CoresController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cores>>> GetCores()
        {
            return await _context.Cores.ToListAsync();
        }

        // GET: api/CoresController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cores>> GetCor(int id)
        {
            var cor = await _context.Cores.FindAsync(id);
            if (cor == null) return NotFound();
            return cor;
        }

        // POST: api/CoresController2
        [HttpPost]
        public async Task<ActionResult<Cores>> PostCor([FromBody] CorDto dto)
        {
            var novaCor = new Cores { Nome = dto.Nome };
            _context.Cores.Add(novaCor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCor), new { id = novaCor.Id }, novaCor);
        }

        // PUT: api/CoresController2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCor(int id, [FromBody] CorDto dto)
        {
            var cor = await _context.Cores.FindAsync(id);
            if (cor == null) return NotFound();

            cor.Nome = dto.Nome;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/CoresController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCor(int id)
        {
            var cor = await _context.Cores.FindAsync(id);
            if (cor == null) return NotFound();

            _context.Cores.Remove(cor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class CorDto
    {
        public string Nome { get; set; }
    }
}
