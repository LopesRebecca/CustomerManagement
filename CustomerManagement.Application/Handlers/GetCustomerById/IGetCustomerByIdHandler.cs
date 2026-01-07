using CustomerManagement.Application.Queries.GetClientById;
using CustomerManagement.Application.Queries.GetCustomerResultById.DTO;

namespace CustomerManagement.Application.Handlers.GetCustomerById
{
    public interface IGetCustomerByIdHandler
    {
        Task<CustomerResultDTO?> HandleAsync(GetCustomerByIdQuery request);
    }
}
