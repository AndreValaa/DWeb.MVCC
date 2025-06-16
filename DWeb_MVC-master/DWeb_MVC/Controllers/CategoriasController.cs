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
using Microsoft.AspNetCore.Mvc.Filters;

namespace DWeb_MVC.Controllers
{

    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var email = context.HttpContext.User.Identity?.Name?.ToLower();
            if (email != "jose1@gmail.com")
            {
                context.Result = new RedirectToActionResult("UserHome", "Home", null);
            }

            base.OnActionExecuting(context);
        }

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias
                                  .Include(c => c.Grupos)
                                  .ToListAsync();

            return View(categorias);
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categorias = await _context.Categorias
                .Include(c => c.ListaProdutos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            ViewData["ListaGrupos"] = new SelectList(_context.Grupos.OrderBy(g => g.Nome), "Id", "Nome");
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome, GruposId")] Categorias categorias)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ListaGrupos"] = new SelectList(_context.Grupos.OrderBy(g => g.Nome), "Id", "Nome", categorias.GruposId);

            return View(categorias);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categorias = await _context.Categorias
                .Include(c => c.ListaProdutos) 
                .ThenInclude(p => p.Fotos) 
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorias == null)
            {
                return NotFound();
            }
            return View(categorias);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Categorias categorias)
        {
            if (id != categorias.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriasExists(categorias.Id))
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
            return View(categorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int produtoId, int categoriaId)
        {
            var produto = await _context.Produtos.FindAsync(produtoId);
            if (produto == null)
            {
                return NotFound();
            }

            // Remover o produto da categoria
            var categoria = await _context.Categorias.Include(c => c.ListaProdutos).FirstOrDefaultAsync(c => c.Id == categoriaId);
            if (categoria != null)
            {
                categoria.ListaProdutos.Remove(produto);
                await _context.SaveChangesAsync();
            }

            // Redirecionar para a página de edição da categoria
            return RedirectToAction("Edit", new { id = categoriaId });
        }


        // GET: Categorias/EditProdutoInCategoria/5
        public async Task<IActionResult> EditProdutoInCategoria(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            // Obter o produto pelo id
            var produto = await _context.Produtos
                .Include(p => p.Categoria) // Inclui as categorias associadas ao produto
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            // Passar o produto para a view de edição do produto
            ViewData["ListaCat"] = _context.Categorias.OrderBy(c => c.Nome).ToList();
            return View(produto);
        }

        // POST: Categorias/EditProdutoInCategoria/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProdutoInCategoria(int id, [Bind("Id,Nome,Marca,Preco,PrecoAux")] Produtos produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            // Atualizar as informações do produto (sem mexer nas categorias associadas)
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = produto.Categoria.First().Id });
            }

            ViewData["ListaCat"] = _context.Categorias.OrderBy(c => c.Nome).ToList();
            return View(produto);
        }

        // GET: Categorias/DeleteProdutoInCategoria/5
        public async Task<IActionResult> DeleteProdutoInCategoria(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            // Buscar o produto pelo ID
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            // Remover o produto da categoria
            var categoria = produto.Categoria.FirstOrDefault();
            if (categoria != null)
            {
                categoria.ListaProdutos.Remove(produto);
            }

            // Salvar as alterações na base de dados
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            // Redirecionar para a página de edição da categoria
            return RedirectToAction(nameof(Edit), new { id = categoria.Id });
        }


        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categorias = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categorias' is null.");
            }

            var categoria = await _context.Categorias
                .Include(c => c.ListaProdutos) // Inclui os produtos associados
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria != null)
            {
                // Verifica se algum produto associado tem apenas uma categoria
                bool canDelete = true;
                foreach (var produto in categoria.ListaProdutos)
                {
                    // Contar quantas categorias o produto tem
                    var categoriaCount = await _context.Categorias.CountAsync(c => c.ListaProdutos.Any(p => p.Id == produto.Id));
                    if (categoriaCount <= 1)
                    {
                        canDelete = false;
                        break; // Não pode deletar a categoria
                    }
                }

                if (!canDelete)
                {
                    ModelState.AddModelError("", "Não é possível excluir a categoria, pois alguns produtos estão vinculados a ela como sua única categoria.");
                    return View(categoria); // Retorna à view de exclusão com a mensagem de erro
                }

                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CategoriasExists(int id)
        {
          return (_context.Categorias?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

    }
}
