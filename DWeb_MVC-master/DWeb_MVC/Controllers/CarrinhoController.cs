﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Models;
using DWeb_MVC.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;


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

        [HttpPost]

        public IActionResult AdicionarAoCarrinho([FromBody] JsonElement data)
        {
            if (!data.TryGetProperty("ProdutoId", out var produtoIdElem) ||
                !data.TryGetProperty("Cor", out var corElem) ||
                !data.TryGetProperty("Tamanho", out var tamanhoElem) ||
                produtoIdElem.ValueKind != JsonValueKind.Number ||
                corElem.ValueKind != JsonValueKind.String ||
                tamanhoElem.ValueKind != JsonValueKind.String)
            {
                return Json(new { sucesso = false, mensagem = "Dados inválidos" });
            }

            int produtoId = produtoIdElem.GetInt32();
            string cor = corElem.GetString();
            string tamanho = tamanhoElem.GetString();

            var produto = _context.Produtos
                .Include(p => p.Fotos)
                .FirstOrDefault(p => p.Id == produtoId);

            if (produto == null)
            {
                return Json(new { sucesso = false, mensagem = "Produto não encontrado" });
            }

            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho")
                           ?? new List<CarrinhoItem>();

            var itemExistente = carrinho.FirstOrDefault(c =>
                c.ProdutoId == produtoId &&
                c.Cor == cor &&
                c.Tamanho == tamanho);

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
                    Imagem = produto.Fotos.FirstOrDefault()?.NomeFicheiro ?? "noProduto.jpg",
                    Cor = cor,
                    Tamanho = tamanho
                });
            }

            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

            return Json(new { sucesso = true, mensagem = "Produto adicionado ao carrinho com sucesso" });
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

            var produtosFormatados = string.Join(", ", carrinho.Select(c =>
                $"{c.Nome} (x{c.Quantidade}) Cor: {c.Cor}, Tamanho: {c.Tamanho}"
            ));

            var compra = new Compras
            {
                Email = userEmail,
                ProdutosComprados = produtosFormatados,
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
