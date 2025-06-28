using System.ComponentModel.DataAnnotations.Schema;

namespace DWeb_MVC.Models
{
    public class Compras
    {
        public int Id { get; set; }


        public string Email { get; set; }


        public string ProdutosComprados { get; set; }


        public decimal PrecoTotal { get; set; }


        public int QuantidadeTotal { get; set; }


        public DateTime DataCompra { get; set; }
    }
}

