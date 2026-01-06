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

        public async Task CreateAsync(ClientEntity cliente)
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            await session.SaveAsync(cliente);
            await transaction.CommitAsync();
        }

        public async Task<ClientEntity?> GetByIdAsync(int id)
        {
            using var session = _sessionFactory.OpenSession();
            return await session.GetAsync<ClientEntity>(id);
        }

        public async Task<bool> ExistDocumentNumberAsync(DocumentNumber documento)
        {
            using var session = _sessionFactory.OpenSession();

            return await session.Query<ClientEntity>()
                .AnyAsync(c =>
                    c.DocumentNumber.Valor == documento.Valor
                );
        }
    }
}
