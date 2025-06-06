using OrderManagement.Dominio;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain
{
    public class Produto : Entidade
    {
        [Required]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Required]
        public double Preco { get; set; }

        [Required]
        public int QuantidadeDisponivel { get; set; }
    }
}
