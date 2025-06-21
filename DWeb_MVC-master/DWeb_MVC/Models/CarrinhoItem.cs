namespace DWeb_MVC.Models
{
    public class CarrinhoItem
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public string Cor { get; set; }
        public string Tamanho { get; set; }

        public string Imagem { get; set; }
    }
}
