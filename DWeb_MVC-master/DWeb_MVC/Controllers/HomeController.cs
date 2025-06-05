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

        public async Task<IActionResult> Index()
        {
            /* procurar, na base de dados, a lista dos produtos existentes
          * SELECT *
          * FROM Produtos a INNER JOIN Categoria c ON a.Categoria = c.Id
          */
            //var listaProdutos = await _bd.Produtos.Include(p => p.Categoria).ToListAsync();
            //return View(new { products = listaProdutos });
            return Redirect("/Identity/Account/Login");

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