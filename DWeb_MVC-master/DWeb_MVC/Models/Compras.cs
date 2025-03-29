using System.ComponentModel.DataAnnotations.Schema;

namespace DWeb_MVC.Models
{
    public class Compras{

        public int Id { get; set; }
        /// <summary>
        /// Comfirmação se cliente pagou a compra
        /// </summary>
        public bool Pago { get; set; }

        //******************************

        [ForeignKey(nameof(Clientes))]
        public int ClientesFK { get; set; }
        public Clientes Cliente { get; set; }


        public List<DetalhesCompras> DetalhesCompras { get; set; } = new();
	}
}
