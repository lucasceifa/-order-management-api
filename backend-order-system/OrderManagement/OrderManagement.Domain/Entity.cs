using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dominio
{
    public class Entity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
    }
}
