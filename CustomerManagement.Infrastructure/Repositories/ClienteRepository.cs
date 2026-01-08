using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;
using NHibernate;
using NHibernate.Linq;

namespace CustomerManagement.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _sessao;

        public ClienteRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            _sessao = _sessionFactory.OpenSession();
        }

        public async Task CriarAsync(Cliente cliente, CancellationToken cancellationToken = default)
        {
            using var transacao = _sessao.BeginTransaction();

            await _sessao.SaveAsync(cliente, cancellationToken);
            await transacao.CommitAsync(cancellationToken);
        }

        public async Task<Cliente?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _sessao.GetAsync<Cliente>(id, cancellationToken);
        }

        public async Task<bool> ExisteNumeroDocumentoAsync(NumeroDocumento documento, CancellationToken cancellationToken = default)
        {
            return await _sessao.Query<Cliente>()
                .AnyAsync(c => c.NumeroDocumento.Valor == documento.Valor && 
                c.NumeroDocumento.Tipo == documento.Tipo, 
                cancellationToken);
        }
    }
}
