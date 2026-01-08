using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Interface.Repositories
{
    public interface IClienteRepository
    {
        Task CriarAsync(Cliente cliente, CancellationToken cancellationToken = default);
        Task<Cliente?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExisteNumeroDocumentoAsync(NumeroDocumento documento, CancellationToken cancellationToken = default);
    }
}
