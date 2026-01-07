using CustomerManagement.Application.Queries.GetClientById;
using CustomerManagement.Application.Queries.GetClientById.DTO;

namespace CustomerManagement.Application.Handlers.GetClientById
{
    public interface IGetClientByIdHandler
    {
        Task<ClientResultDTO?> HandleAsync(GetClientByIdQuery request);
    }
}
