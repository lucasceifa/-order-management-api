using OrderManagement.Dominio.Enums;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dominio
{
    public class OrderXProduct : Entity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public int QuantityPurchased { get; set; }

        [Required]
        public double ProductValue { get; set; }
    }
}
