using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dominio
{
    public class Entidade
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime DataDeCadastro { get; set; }

        public Entidade()
        {
            Id = Guid.NewGuid();
            DataDeCadastro = DateTime.Now;
        }
    }
}
