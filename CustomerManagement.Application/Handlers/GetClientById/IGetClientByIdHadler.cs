using CustomerManagement.Application.Queries.GetClientById;
using CustomerManagement.Application.Queries.GetClientById.DTO;

namespace CustomerManagement.Application.Handlers.GetClientById
{
    public interface IGetClientByIdHadler
    {
        Task<ClientResultDTO?> HandleAsync(GetClientByIdQuery request);
    }
}
