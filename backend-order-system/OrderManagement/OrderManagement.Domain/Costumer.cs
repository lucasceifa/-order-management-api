using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dominio
{
    public class Customer : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Cellphone { get; set; }

        public Customer() { }

        public Customer(CustomerInput input)
        {
            Name = input.Name;
            Email = input.Email;
            Cellphone = input.Cellphone;
        }
    }
}
