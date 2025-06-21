using DWeb_MVC.Data;
using DWeb_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DWeb_MVC.Controllers
{
    [Authorize(Roles = "admin")]

    public class CoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        

        public CoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cores.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cor = await _context.Cores
                .Include(c => c.ListaProdutos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cor == null) return NotFound();

            return View(cor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Cores cor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cor = await _context.Cores.FindAsync(id);
            if (cor == null) return NotFound();

            return View(cor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Cores cor)
        {
            if (id != cor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoresExists(cor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cor = await _context.Cores.FirstOrDefaultAsync(m => m.Id == id);
            if (cor == null) return NotFound();

            return View(cor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cor = await _context.Cores
                .Include(c => c.ListaProdutos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cor != null && cor.ListaProdutos.Any())
            {
                ModelState.AddModelError("", "Não é possível remover esta cor pois está associada a produtos.");
                return View(cor);
            }

            _context.Cores.Remove(cor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoresExists(int id)
        {
            return _context.Cores.Any(e => e.Id == id);
        }
    }
}
