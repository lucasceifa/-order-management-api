using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Requests
{
    public class OrderXProductInput
    {
        public Guid CostumerId { get; set; }
        public List<ProductXQuantities> ProductXQuantities { get; set; }
    }

    public class ProductXQuantities
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
