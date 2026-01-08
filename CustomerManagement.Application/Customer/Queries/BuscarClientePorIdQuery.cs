using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Mediator;

namespace CustomerManagement.Application.Customer.Queries
{
    public class BuscarClientePorIdQuery : IRequest<BuscarClientePorIdResultadoDTO?>
    {
        public required int Id { get; init; }
    }
}
