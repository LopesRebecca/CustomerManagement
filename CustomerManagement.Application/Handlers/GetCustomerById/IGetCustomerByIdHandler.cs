using CustomerManagement.Application.Queries.GetClientById;
using CustomerManagement.Application.Queries.GetCustomerResulById.DTO;

namespace CustomerManagement.Application.Handlers.GetCustomerById
{
    public interface IGetCustomerByIdHandler
    {
        Task<CustomerResultDTO?> HandleAsync(GetCustomerByIdQuery request);
    }
}
