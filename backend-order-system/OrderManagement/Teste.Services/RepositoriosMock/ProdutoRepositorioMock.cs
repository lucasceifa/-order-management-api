using OrderManagement.Domain;
using OrderManagement.Dominio;
using OrderManagement.Dominio.Interfaces;
using OrderManagement.Dominio.Utils;

namespace OrderManagement.API.Teste.Services.RepositoriosMock
{
    public class ProdutoRepositorioMock : IProdutoRepositorio
    {
        private List<Produto> _dados;

        public ProdutoRepositorioMock()
        {
            if (_dados == null)
                _dados = new List<Produto>();
        }

        public async Task AtualizarAsync(Produto produto)
        {
            var index = _dados.FindIndex(p => p.Id == produto.Id);
            if (index >= 0)
                _dados[index] = produto;
        }

        public async Task CriarAsync(Produto produto)
        {
            _dados.Add(produto);
        }

        public async Task DeletarAsync(Guid id)
        {
            _dados = _dados.Where(p => p.Id != id).ToList();
        }

        public async Task<IEnumerable<Produto>> ObterAsync(ParametrosBuscaProduto filtro)
        {
            return _dados.Where(p =>
                (string.IsNullOrEmpty(filtro.Nome) || p.Nome.ToLower().Contains(filtro.Nome.ToLower())) &&
                (!filtro.PrecoMin.HasValue || p.Preco >= filtro.PrecoMin.Value) &&
                (!filtro.PrecoMax.HasValue || p.Preco <= filtro.PrecoMax.Value) &&
                (!filtro.QuantidadeDisponivel.HasValue || p.QuantidadeDisponivel == filtro.QuantidadeDisponivel.Value));
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return _dados.FirstOrDefault(p => p.Id == id);
        }
    }
}
