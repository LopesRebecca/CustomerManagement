using CustomerManagement.Application.Commands.Request;
using CustomerManagement.Application.Commands.Response;

namespace CustomerManagement.Application.Handlers.CreateClient
{
    public interface ICreateClientHandler
    {
        Task<CreateClientResponse> HandleAsync(CreateClientRequest command);
    }
}
