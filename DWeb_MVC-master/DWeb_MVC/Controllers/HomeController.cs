using DWeb_MVC.Data;
using DWeb_MVC.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;


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
            var produtos = await _bd.Produtos
      .Include(p => p.Categoria).ThenInclude(c => c.Grupos)
      .Include(p => p.Fotos)
      .Take(10) 
      .ToListAsync();


            ViewBag.GruposComCategorias = produtos
                .SelectMany(p => p.Categoria)
                .Where(c => c.Grupos != null)
                .GroupBy(c => c.Grupos.Nome)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Nome).Distinct().ToList());

            ViewBag.ProdutosPorGrupo = produtos
                .GroupBy(p => p.Categoria.FirstOrDefault()?.Grupos?.Nome ?? "Outros")
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(produtos);
        }

        public IActionResult SobreNos()
        {
            return View();
        }




        [HttpGet]
        public async Task<IActionResult> GetCoresTamanhos(int id)
        {
            var produto = await _bd.Produtos
                .Include(p => p.Cores)
                .Include(p => p.Tamanhos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return NotFound();

            var resultado = new
            {
                cores = produto.Cores.Select(c => c.Nome).ToList(),
                tamanhos = produto.Tamanhos.Select(t => t.Nome).ToList()
            };

            var json = System.Text.Json.JsonSerializer.Serialize(resultado, new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });

            return Json(resultado);

        }





        public async Task<IActionResult> ProdutosPorGrupo(string grupo)
        {
            if (string.IsNullOrEmpty(grupo)) return NotFound();

            var produtos = await _bd.Produtos
                .Include(p => p.Categoria).ThenInclude(c => c.Grupos)
                .Include(p => p.Fotos)
                .Where(p => p.Categoria.Any(c => c.Grupos.Nome == grupo))
                .ToListAsync();

            ViewBag.GruposComCategorias = await _bd.Categorias
                .Include(c => c.Grupos)
                .Where(c => c.Grupos != null)
                .GroupBy(c => c.Grupos.Nome)
                .ToDictionaryAsync(g => g.Key, g => g.Select(c => c.Nome).Distinct().ToList());

            ViewBag.GrupoNome = grupo;
            return View(produtos);
        }


        public async Task<IActionResult> ProdutosPorCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
            {
                return RedirectToAction("UserHome");
            }

            var produtos = await _bd.Produtos
                .Include(p => p.Categoria).ThenInclude(c => c.Grupos)
                .Include(p => p.Fotos)
                .Where(p => p.Categoria.Any(c => c.Nome == categoria))
                .ToListAsync();

            ViewBag.GruposComCategorias = await _bd.Categorias
                .Include(c => c.Grupos)
                .Where(c => c.Grupos != null)
                .GroupBy(c => c.Grupos.Nome)
                .ToDictionaryAsync(g => g.Key, g => g.Select(c => c.Nome).Distinct().ToList());

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
            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
            {
                // Admin → vai para a view admin
                return View("Index");
            }

            // Qualquer outro → redireciona para UserHome
            return RedirectToAction("UserHome", "Home");
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