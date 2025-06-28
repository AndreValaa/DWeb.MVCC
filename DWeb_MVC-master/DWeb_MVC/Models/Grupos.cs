using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DWeb_MVC.Models
{
    public class Grupos
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nome { get; set; }

        // Relação: 1 Grupos -> N Categorias
        public ICollection<Categorias> ListaCategorias { get; set; }

    }
}
