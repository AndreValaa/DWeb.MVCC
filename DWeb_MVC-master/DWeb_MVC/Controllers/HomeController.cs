using DWeb_MVC.Data;
using DWeb_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DWeb_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _bd;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ApplicationDbContext context,
            ILogger<HomeController> logger
            )
        {
            _bd = context;
            _logger = logger;
        }

        public async Task<IActionResult> UserHome()
        {
            var listaProdutos = await _bd.Produtos
                .Include(p => p.Categoria)
                    .ThenInclude(c => c.Grupos)
                .Include(p => p.Fotos)
                .ToListAsync();

            return View(listaProdutos);
        }

        public async Task<IActionResult> ProdutosPorGrupo(string grupo)
        {
            if (string.IsNullOrEmpty(grupo)) return NotFound();

            var produtos = await _bd.Produtos
                .Include(p => p.Categoria)
                    .ThenInclude(c => c.Grupos)
                .Include(p => p.Fotos)
                .Where(p => p.Categoria.Any(c => c.Grupos.Nome == grupo))
                .ToListAsync();

            ViewBag.GrupoNome = grupo;
            return View(produtos);
        }

        public async Task<IActionResult> ProdutosPorCategoria(string categoria)
        {
            if (string.IsNullOrEmpty(categoria)) return NotFound();

            var produtos = await _bd.Produtos
                .Include(p => p.Categoria)
                    .ThenInclude(c => c.Grupos)
                .Include(p => p.Fotos)
                .Where(p => p.Categoria.Any(c => c.Nome == categoria))
                .ToListAsync();

            ViewBag.Categoria = categoria;
            return View(produtos);
        }



        public async Task<IActionResult> UserProdutos()
        {
            var listaProdutos = await _bd.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fotos)
                .ToListAsync();

            return View(listaProdutos);
        }



        public async Task<IActionResult> Index()
        {

          
                if (User.Identity.Name?.ToLower() != "jose1@gmail.com")
                {
                    return RedirectToAction("UserHome");
                }

                /* procurar, na base de dados, a lista dos produtos existentes
              * SELECT *
              * FROM Produtos a INNER JOIN Categoria c ON a.Categoria = c.Id
              */
                var listaProdutos = await _bd.Produtos.Include(p => p.Categoria).ToListAsync();
            return View(new { products = listaProdutos });
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}