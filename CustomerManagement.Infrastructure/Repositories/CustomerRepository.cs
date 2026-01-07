using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;
using NHibernate;
using NHibernate.Linq;

namespace CustomerManagement.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;

        public CustomerRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            _session = _sessionFactory.OpenSession();
        }

        public async Task CreateAsync(CustomerEntity cliente, CancellationToken cancellationToken = default)
        {
            using var transaction = _session.BeginTransaction();

            await _session.SaveAsync(cliente, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task<CustomerEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _session.GetAsync<CustomerEntity>(id, cancellationToken);
        }

        public async Task<bool> ExistDocumentNumberAsync(DocumentNumber document, CancellationToken cancellationToken = default)
        {
            return await _session.Query<CustomerEntity>()
                .AnyAsync(c => c.DocumentNumber.Value == document.Value && c.DocumentNumber.Type == document.Type, cancellationToken);
        }
    }
}
