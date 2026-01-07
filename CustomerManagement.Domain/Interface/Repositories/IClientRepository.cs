using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Interface.Repositories
{
    public interface IClientRepository
    {
        Task CreateAsync(ClientEntity client, CancellationToken cancellationToken = default);
        Task<ClientEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistDocumentNumberAsync(DocumentNumber document, CancellationToken cancellationToken = default);
    }
}
