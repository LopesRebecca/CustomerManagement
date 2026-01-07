using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Interface.Repositories
{
    public interface ICustomerRepository
    {
        Task CreateAsync(CustomerEntity client, CancellationToken cancellationToken = default);
        Task<CustomerEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistDocumentNumberAsync(DocumentNumber document, CancellationToken cancellationToken = default);
    }
}
