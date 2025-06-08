using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Dominio.Utils
{
    public class SearchfilterProduct
    {
        public string? Name { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
        public int? QuantityAvailable { get; set; }
    }
}
