using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Dominio.Utils
{
    public class ParametrosBuscaCliente
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime? DataDeCadastro { get; set; }
    }
}
