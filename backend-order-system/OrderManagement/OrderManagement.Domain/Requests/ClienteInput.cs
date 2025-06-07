using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OrderManagement.Dominio
{
    public class ClienteInput
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string? Telefone { get; set; }

        public bool Validate()
        {
            if (String.IsNullOrEmpty(Nome) || Nome.Length < 2 || String.IsNullOrEmpty(Email) || Email.Length < 5)
                return false;

            if (Nome.Split(" ").Count() < 2)
                return false;

            if (!Regex.IsMatch(Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$"))
                return false;

            return true;
        }
    }
}