using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DWeb_MVC.Data;
using DWeb_MVC.Models;

namespace DWeb_MVC.Controllers.API
{
    [Route("api/[controller]")]
    public class ComprasController2 : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController2(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ComprasController2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compras>>> GetCompras()
        {
            if (_context.Compras == null)
            {
                return NotFound();
            }

            return await _context.Compras.ToListAsync();
        }

        // POST: api/ComprasController2
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras([FromBody] CompraDto compraDto)
        {
            if (_context.Compras == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Compras' is null.");
            }

            var compra = new Compras
            {
                Email = compraDto.Email,
                ProdutosComprados = compraDto.ProdutosComprados,
                PrecoTotal = compraDto.PrecoTotal,
                QuantidadeTotal = compraDto.QuantidadeTotal,
                DataCompra = DateTime.Now
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompras), new { id = compra.Id }, compra);
        }
    }

    public class CompraDto
    {
        public string Email { get; set; }
        public string ProdutosComprados { get; set; }
        public decimal PrecoTotal { get; set; }
        public int QuantidadeTotal { get; set; }
    }
}
