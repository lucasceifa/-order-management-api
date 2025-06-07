using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Dominio.Utils
{
    public class ParametrosBuscaProduto
    {
        public string? Nome { get; set; }
        public double? PrecoMin { get; set; }
        public double? PrecoMax { get; set; }
        public int? QuantidadeDisponivel { get; set; }
    }
}
