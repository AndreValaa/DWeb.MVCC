using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWeb_MVC.Models
{
    public class Clientes{

        public List<Compras> Compras { get; set; } = new List<Compras>();

        public int Id { get; set; }


        /// <summary>
        /// Nome do Cliente
        /// </summary>
        [StringLength(50)]
        public string Nome { get; set; }


        /// <summary>
        /// Número de telemóvel do cliente
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Telemóvel")]
        [StringLength(9, MinimumLength = 9,
                    ErrorMessage = "O {0} deve ter {1} dígitos")]
        [RegularExpression("9[1236][0-9]{7}",
                    ErrorMessage = "O número de {0} deve começar por 91, 92, 93 ou 96, e ter 9 dígitos")]
        public string Telemovel { get; set; }


        /// <summary>
        /// Endereço de email do cliente
        /// </summary>
        [EmailAddress(ErrorMessage = "O {0} não está corretamente escrito")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [RegularExpression("[a-z._0-9]+@gmail.com", ErrorMessage = "O {0} tem de ser do GMail")]
        [StringLength(40)]
        public string Email { get; set; }


        /// <summary>
        /// Morada do cliente
        /// </summary>
        [StringLength(100)]
        public string Morada { get; set; }


        /// <summary>
        /// Código postal do cliente
        /// </summary>
        [DisplayName("Código Postal")]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-ZÇÁÉÍÓÚÊÂÎÔÛÀÃÕ ]+",
                         ErrorMessage = "O {0} tem de ser da forma XXXX-XXX NOME DA TERRA")]
        [StringLength(25)]
        public string CodPostal { get; set; }

        // *****************************************************
        /// <summary>
        /// atributo para efetuar a ligação entre a base 
        /// de dados do 'negócio' e a base de dados da autenticação
        /// </summary>
        public string UserId { get; set; }
    }
}
