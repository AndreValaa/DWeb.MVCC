using System.ComponentModel.DataAnnotations;

namespace DWeb_MVC.Models
{
    public class Categorias{
        public int Id { get; set; }
        /// <summary>
        /// Nome da categoria
        /// </summary>
        [StringLength(50)]
        public string Nome { get; set;}

        public ICollection<Produtos> ListaProdutos { get; set; } 

    }
}
