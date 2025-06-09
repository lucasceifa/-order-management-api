using OrderManagement.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Responses
{
    public class ProductsOrderResponse
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Value { get; set; }
        public double TotalValue { get; set; }

        public ProductsOrderResponse(OrderXProduct orderXProduct, Product product)
        {
            ProductName = product.Name;
            Quantity = orderXProduct.QuantityPurchased;
            Value = orderXProduct.ProductValue;
            TotalValue = Quantity * Value;
        }

    }
}
