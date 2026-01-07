using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Mediator;

namespace CustomerManagement.Application.Customer.Commands
{
    public class CreateCustomerCommand : IRequest<CreateCustomerResultDTO>
    {
        public required string Name { get; set; }
        public required string DocumentNumber { get; set; }
    }
}
