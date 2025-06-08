using OrderManagement.Dominio.Enums;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Dominio
{
    public class OperationCostumerXProduct : Entity
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid CostumerId { get; set; }

        [Required]
        public int QuantityPurchased { get; set; }

        [Required]
        public double ProductValue { get; set; }
    }
}
