using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;
using NHibernate;
using NHibernate.Linq;

namespace CustomerManagement.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public ClientRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task CreateAsync(ClientEntity cliente, CancellationToken cancellationToken = default)
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            await session.SaveAsync(cliente, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task<ClientEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var session = _sessionFactory.OpenSession();
            return await session.GetAsync<ClientEntity>(id, cancellationToken);
        }

        public async Task<bool> ExistDocumentNumberAsync(DocumentNumber documento, CancellationToken cancellationToken = default)
        {
            using var session = _sessionFactory.OpenSession();

            return await session.Query<ClientEntity>()
                .AnyAsync(c => c.DocumentNumber.Valor == documento.Valor, cancellationToken);
        }
    }
}
