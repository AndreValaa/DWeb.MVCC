using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWeb_MVC.Models
{
    public class Produtos{		
		public Produtos()
        {
            Fotos = new HashSet<Fotografias>();  
            DetalhesCompras=new HashSet<DetalhesCompras>();
        }

        public int Id { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        [StringLength(50)]
        public string Nome { get; set; }

        /// <summary>
        /// Marca do produto
        /// </summary>
        [StringLength(50)]
        public string Marca { get; set;}

        /// <summary>
        /// Preço do produto por unidade
        /// </summary>
        public decimal Preco  { get; set; }

        [NotMapped] //esta anotaçao impede a EF de exportar este atributo
        [RegularExpression("[0-9]+(.|,)?[0-9]{0,2}",
         ErrorMessage = "Só pode escrever algarismos e, " +
         "se desejar, duas casas decimais no {0}")]
        [Display(Name = "preco")]
        public string PrecoAux { get; set; }

        /// <summary>
        /// lista das fotografias associadas a um Produto
        /// </summary>
        public ICollection<Fotografias> Fotos { get; set; }

        //************************************

        /// <summary>
        /// FK para Categorias
        /// </summary>
        //[ForeignKey(nameof(Categoria))]
        //public int Categoria { get; set; }
        //public Categorias Categoria { get; set; }
        public ICollection<Categorias> Categoria { get; set; }

        public ICollection<Cores> Cores { get; set; }


        public ICollection<DetalhesCompras> DetalhesCompras { get; set; }
	}
}
