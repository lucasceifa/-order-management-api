using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.API.Teste.Services.RepositorysMock
{
    public class CostumerRepositoryMock : ICostumerRepository
    {
        private List<Costumer> _dados;

        public CostumerRepositoryMock()
        {
            if (_dados == null)
                _dados = new List<Costumer>();
        }

        public async Task UpdateAsync(Costumer Costumer)
        {
            var index = _dados.FindIndex(c => c.Id == Costumer.Id);
            _dados[index] = Costumer;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return _dados.Any(c => c.Email == email);
        }

        public async Task CreateAsync(Costumer Costumer)
        {
            _dados.Add(Costumer);
        }

        public async Task DeleteAsync(Guid id)
        {
            _dados = _dados.Where(c => c.Id != id).ToList();
        }

        public async Task<IEnumerable<Costumer>> GetAsync(SearchfilterCostumer filter)
        {
            return _dados.Where(e => (!filter.CreationDate.HasValue || e.CreationDate > filter.CreationDate) && (String.IsNullOrEmpty(filter.Email) || e.Email.ToLower().Contains(filter.Email)) || (String.IsNullOrEmpty(filter.Name) || e.Name.ToLower().Contains(filter.Name)));
        }

        public async Task<Costumer?> GetByIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(c => c.Id == id);
        }
    }
}
