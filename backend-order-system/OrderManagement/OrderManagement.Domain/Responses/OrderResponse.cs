using OrderManagement.Dominio;
using OrderManagement.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string? CancellationDate { get; set; }
        public string CreationDate { get; set; }
        public double TotalValue { get; set; }

        public OrderResponse(Order order, Customer customer, List<OrderXProduct> orderItems)
        {
            Id = order.Id;
            CustomerId = customer.Id;
            Status = order.GetStatus();
            CustomerName = customer.Name;
            CreationDate = order.CreationDate.ToString("dd/MM/yyyy");
            CancellationDate = order.CancellationDate?.ToString("dd/MM/yyyy");
            TotalValue = orderItems.Sum(e => e.QuantityPurchased * e.ProductValue);
        }

    }
}
