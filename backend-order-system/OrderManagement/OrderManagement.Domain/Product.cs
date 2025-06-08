using OrderManagement.Dominio;
using OrderManagement.Dominio.Requests;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain
{
    public class Product : Entity
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int QuantityAvailable { get; set; }

        public Product() { }

        public Product(ProductInput input)
        {
            Name = input.Name;
            Description = input.Description;
            Price = input.Price;
            QuantityAvailable = input.QuantityAvailable;
        }
    }
}
