using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DWeb_MVC.Models
{
    public class Fotografias
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome do documento com a fotografia do cão/cadela
        /// </summary>
        public string NomeFicheiro { get; set; }

        /// <summary>
        /// data em que a fotografia foi tirada
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// local onde a fotografia foi tirada
        /// </summary>
        public string Local { get; set; }

        //*********************************************

        /// <summary>
        /// FK para identificar o Produto a quem a Fotografia pertence 
        /// </summary>
        [ForeignKey(nameof(Produto))]  // <=>  [ForeignKey("Produto")]
        public int ProdutoFK { get; set; }
        [JsonIgnore]
        public Produtos Produto { get; set; }
    }
}