using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Mediator;

namespace CustomerManagement.Application.Customer.Queries
{
    public class GetCustomerByIdQuery : IRequest<GetCustomerByIdResultDTO?>
    {
        public required int Id { get; init; }
    }
}
