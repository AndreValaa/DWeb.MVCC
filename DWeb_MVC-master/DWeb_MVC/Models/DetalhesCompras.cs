using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWeb_MVC.Models
{
	[PrimaryKey(nameof(CompraFK), nameof(ProdutoFK))]
	public class DetalhesCompras
	{

		//public int Id { get; set; }

		public int Quantidade { get; set; }

		public decimal PrecoCompra { get; set; }

		public decimal IVA { get; set; }


		//************************************


		[ForeignKey(nameof(Compra))]
		public int CompraFK { get; set; }
		public Compras Compra { get; set; }


		[ForeignKey(nameof(Produto))]
		public int ProdutoFK { get; set; }
		public Produtos Produto { get; set; }

	}

}
