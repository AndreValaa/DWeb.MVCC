using System.ComponentModel.DataAnnotations;

namespace DWeb_MVC.Models
{
    public class Cores
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }
    }
}
