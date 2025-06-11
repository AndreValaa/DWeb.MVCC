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
    public class CoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cores
        public async Task<IActionResult> Index()
        {
              return View(await _context.Cores.ToListAsync());
        }

        // GET: Cores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cores == null)
            {
                return NotFound();
            }

            var cores = await _context.Cores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cores == null)
            {
                return NotFound();
            }

            return View(cores);
        }

        // GET: Cores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Cores cores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cores);
        }

        // GET: Cores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cores == null)
            {
                return NotFound();
            }

            var cores = await _context.Cores.FindAsync(id);
            if (cores == null)
            {
                return NotFound();
            }
            return View(cores);
        }

        // POST: Cores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Cores cores)
        {
            if (id != cores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoresExists(cores.Id))
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
            return View(cores);
        }

        // GET: Cores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cores == null)
            {
                return NotFound();
            }

            var cores = await _context.Cores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cores == null)
            {
                return NotFound();
            }

            return View(cores);
        }

        // POST: Cores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cores'  is null.");
            }
            var cores = await _context.Cores.FindAsync(id);
            if (cores != null)
            {
                _context.Cores.Remove(cores);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoresExists(int id)
        {
          return _context.Cores.Any(e => e.Id == id);
        }
    }
}
