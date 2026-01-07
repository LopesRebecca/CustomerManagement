using CustomerManagement.Application.Commands.Request;
using CustomerManagement.Application.Commands.Response;

namespace CustomerManagement.Application.Handlers.CreateCustomer
{
    public interface ICreateCustomerHandler
    {
        Task<CreateCustomerResponse> HandleAsync(CreateCustomerRequestCommand command, CancellationToken cancellationToken = default);
    }
}
