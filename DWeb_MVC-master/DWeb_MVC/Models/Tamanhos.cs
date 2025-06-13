﻿using System.ComponentModel.DataAnnotations;

namespace DWeb_MVC.Models
{
    public class Tamanhos    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }
        public ICollection<Produtos> ListaProdutos { get; set; }

    }
}
