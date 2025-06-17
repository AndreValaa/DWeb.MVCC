using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Models;
using DWeb_MVC.Data;
using Microsoft.AspNetCore.Identity;

namespace DWeb_MVC.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarrinhoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho")
                           ?? new List<CarrinhoItem>();
            return View("~/Views/Home/Carrinho.cshtml", carrinho);
        }

        public IActionResult AdicionarAoCarrinho(int id)
        {
            var produto = _context.Produtos
                .Include(p => p.Fotos)
                .FirstOrDefault(p => p.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

            var itemExistente = carrinho.FirstOrDefault(c => c.ProdutoId == id);
            if (itemExistente != null)
            {
                itemExistente.Quantidade++;
            }
            else
            {
                carrinho.Add(new CarrinhoItem
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Quantidade = 1,
                    Imagem = produto.Fotos.FirstOrDefault()?.NomeFicheiro ?? "noProduto.jpg"
                });
            }

            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

            // Voltar para onde estava
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult RemoverDoCarrinho(int id)
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();
            var item = carrinho.FirstOrDefault(i => i.ProdutoId == id);
            if (item != null)
            {
                carrinho.Remove(item);
                HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AtualizarQuantidade(int produtoId, int quantidade)
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();
            var item = carrinho.FirstOrDefault(c => c.ProdutoId == produtoId);
            if (item != null)
            {
                item.Quantidade = quantidade > 0 ? quantidade : 1;
                HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> FinalizarCompra()
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

            if (!carrinho.Any())
            {
                return RedirectToAction("Index");
            }

            var userEmail = User.Identity.Name ?? "anónimo";

            var compra = new Compras
            {
                Email = userEmail,
                ProdutosComprados = string.Join(", ", carrinho.Select(c => $"{c.Nome} (x{c.Quantidade})")),
                PrecoTotal = carrinho.Sum(c => c.Preco * c.Quantidade),
                QuantidadeTotal = carrinho.Sum(c => c.Quantidade),
                DataCompra = DateTime.Now
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Carrinho");

            return RedirectToAction("UserHome", "Home");
        }




    }
}
