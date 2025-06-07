using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Dominio.Requests
{
    public class ProdutoInput
    {
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public double Preco { get; set; }
        public int QuantidadeDisponivel { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Nome) || Nome.Length < 2)
                return false;

            if (Preco < 0)
                return false;

            if (QuantidadeDisponivel < 0)
                return false;

            return true;
        }
    }
}
