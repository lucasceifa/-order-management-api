using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dominio
{
    public class Cliente : Entidade
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Telefone { get; set; }
    }
}
