using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Interface.Repositories
{
    public interface IClientRepository
    {
        Task CreateAsync(ClientEntity cliente);
        Task<ClientEntity?> GetByIdAsync(int id);
        Task<bool> ExistDocumentNumberAsync(DocumentNumber documento);
    }
}
