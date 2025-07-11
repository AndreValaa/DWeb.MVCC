using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController2 : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GruposController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GruposController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrupoDTO>>> GetGrupos()
        {
            return await _context.Grupos
                .Select(g => new GrupoDTO { Id = g.Id, Nome = g.Nome })
                .ToListAsync();
        }

        // GET: api/GruposController2/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoDTO>> GetGrupo(int id)
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            return new GrupoDTO { Id = grupo.Id, Nome = grupo.Nome };
        }

        // POST: api/GruposController2
        [HttpPost]
        public async Task<ActionResult> PostGrupo([FromBody] GrupoDTO dto)
        {
            var grupo = new Grupos { Nome = dto.Nome };
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGrupo), new { id = grupo.Id }, grupo);
        }

        // PUT: api/GruposController2/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupo(int id, [FromBody] GrupoDTO dto)
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            grupo.Nome = dto.Nome;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/GruposController2/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupo(int id)
        {
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            _context.Grupos.Remove(grupo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class GrupoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
