using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dominio
{
    public class Costumer : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Cellphone { get; set; }

        public Costumer() { }

        public Costumer(CostumerInput input)
        {
            Name = input.Name;
            Email = input.Email;
            Cellphone = input.Cellphone;
        }
    }
}
