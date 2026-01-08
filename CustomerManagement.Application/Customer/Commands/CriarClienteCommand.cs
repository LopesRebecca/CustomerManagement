using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Mediator;

namespace CustomerManagement.Application.Customer.Commands
{
    public class CriarClienteCommand : IRequest<CriarClienteResultadoDTO>
    {
        public required string Nome { get; set; }
        public required string NumeroDocumento { get; set; }
    }
}
