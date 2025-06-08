using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OrderManagement.Dominio
{
    public class CostumerInput
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string? Cellphone { get; set; }

        public bool Validate()
        {
            if (String.IsNullOrEmpty(Name) || Name.Length < 2 || String.IsNullOrEmpty(Email) || Email.Length < 5)
                return false;

            if (Name.Split(" ").Count() < 2)
                return false;

            if (!Regex.IsMatch(Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$"))
                return false;

            return true;
        }
    }
}