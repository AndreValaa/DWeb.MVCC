using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers
{
    public class TamanhosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TamanhosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tamanhos
        public async Task<IActionResult> Index()
        {
              return View(await _context.Tamanhos_1.ToListAsync());
        }

        // GET: Tamanhos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tamanhos_1 == null)
            {
                return NotFound();
            }

            var tamanhos = await _context.Tamanhos_1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tamanhos == null)
            {
                return NotFound();
            }

            return View(tamanhos);
        }

        // GET: Tamanhos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tamanhos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Tamanhos tamanhos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tamanhos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tamanhos);
        }

        // GET: Tamanhos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tamanhos_1 == null)
            {
                return NotFound();
            }

            var tamanhos = await _context.Tamanhos_1.FindAsync(id);
            if (tamanhos == null)
            {
                return NotFound();
            }
            return View(tamanhos);
        }

        // POST: Tamanhos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Tamanhos tamanhos)
        {
            if (id != tamanhos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tamanhos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TamanhosExists(tamanhos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tamanhos);
        }

        // GET: Tamanhos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tamanhos_1 == null)
            {
                return NotFound();
            }

            var tamanhos = await _context.Tamanhos_1
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tamanhos == null)
            {
                return NotFound();
            }

            return View(tamanhos);
        }

        // POST: Tamanhos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tamanhos_1 == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tamanhos_1'  is null.");
            }
            var tamanhos = await _context.Tamanhos_1.FindAsync(id);
            if (tamanhos != null)
            {
                _context.Tamanhos_1.Remove(tamanhos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TamanhosExists(int id)
        {
          return _context.Tamanhos_1.Any(e => e.Id == id);
        }
    }
}
