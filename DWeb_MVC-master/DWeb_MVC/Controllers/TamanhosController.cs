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

    public class TamanhosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TamanhosController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tamanhos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tamanho = await _context.Tamanhos
                .Include(t => t.ListaProdutos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tamanho == null) return NotFound();

            return View(tamanho);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Tamanhos tamanho)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tamanho);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tamanho);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tamanho = await _context.Tamanhos.FindAsync(id);
            if (tamanho == null) return NotFound();

            return View(tamanho);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Tamanhos tamanho)
        {
            if (id != tamanho.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tamanho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TamanhosExists(tamanho.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tamanho);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tamanho = await _context.Tamanhos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tamanho == null) return NotFound();

            return View(tamanho);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tamanho = await _context.Tamanhos
                .Include(t => t.ListaProdutos)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tamanho != null && tamanho.ListaProdutos.Any())
            {
                ModelState.AddModelError("", "Não é possível remover este tamanho pois está associado a produtos.");
                return View(tamanho);
            }

            _context.Tamanhos.Remove(tamanho);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TamanhosExists(int id)
        {
            return _context.Tamanhos.Any(e => e.Id == id);
        }
    }
}
