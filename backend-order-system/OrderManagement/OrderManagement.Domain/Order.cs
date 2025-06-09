using OrderManagement.Dominio;
using OrderManagement.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain
{
    public class Order : Entity
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public IOrderStatus Status { get; set; }

        public DateTime? CancellationDate { get; set; }

        public string GetStatus()
        {
            if (Status == IOrderStatus.Canceled) return "Canceled";

            if (Status == IOrderStatus.Concluded) return "Concluded";

            return "";
        }
    }
}
