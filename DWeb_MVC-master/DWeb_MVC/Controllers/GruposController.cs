using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DWeb_MVC.Controllers
{
    public class GruposController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GruposController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Apenas o admin pode aceder
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var email = context.HttpContext.User.Identity?.Name?.ToLower();
            if (email != "jose1@gmail.com")
            {
                context.Result = new RedirectToActionResult("UserHome", "Home", null);
            }
            base.OnActionExecuting(context);
        }

        // GET: Grupos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Grupos.ToListAsync());
        }

        // GET: Grupos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos
                .Include(g => g.ListaCategorias)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grupo == null) return NotFound();

            return View(grupo);
        }

        // GET: Grupos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Grupos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Grupos grupo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grupo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(grupo);
        }

        // GET: Grupos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null) return NotFound();

            return View(grupo);
        }

        // POST: Grupos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Grupos grupo)
        {
            if (id != grupo.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grupo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoExists(grupo.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(grupo);
        }

        // GET: Grupos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var grupo = await _context.Grupos
                .Include(g => g.ListaCategorias)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grupo == null) return NotFound();

            return View(grupo);
        }

        // POST: Grupos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = await _context.Grupos
                .Include(g => g.ListaCategorias)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grupo != null && grupo.ListaCategorias.Any())
            {
                ModelState.AddModelError("", "Não é possível remover este grupo pois está associado a categorias.");
                return View(grupo);
            }

            if (grupo != null)
            {
                _context.Grupos.Remove(grupo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GrupoExists(int id)
        {
            return _context.Grupos.Any(e => e.Id == id);
        }
    }
}
