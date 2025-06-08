using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Dominio.Requests
{
    public class ProductInput
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int QuantityAvailable { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 2)
                return false;

            if (Price < 0)
                return false;

            if (QuantityAvailable < 0)
                return false;

            return true;
        }
    }
}
