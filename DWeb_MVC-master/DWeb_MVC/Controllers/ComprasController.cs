using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace DWeb_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
              return _context.Compras != null ? 
                          View(await _context.Compras.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Compras'  is null.");
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compras = await _context.Compras
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compras == null)
            {
                return NotFound();
            }

            return View(compras);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            ViewData["ClientesFK"] = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Pago,ClientesFK")] Compras compras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientesFK"] = new SelectList(_context.Compras, "Id", "Nome", compras.ClientesFK);
            return View(compras);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compras = await _context.Compras.FindAsync(id);
            if (compras == null)
            {
                return NotFound();
            }
            return View(compras);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Pago,ClientesFK")] Compras compras)
        {
            if (id != compras.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compras);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprasExists(compras.Id))
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
            return View(compras);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compras = await _context.Compras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compras == null)
            {
                return NotFound();
            }

            return View(compras);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Compras == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Compras'  is null.");
            }
            var compras = await _context.Compras.FindAsync(id);
            if (compras != null)
            {
                _context.Compras.Remove(compras);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComprasExists(int id)
        {
          return (_context.Compras?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
