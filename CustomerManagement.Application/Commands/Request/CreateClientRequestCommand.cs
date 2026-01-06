using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Commands.Request
{
    public class CreateClientRequestCommand
    {
        public string Name { get; init; } = default!;
        public string DocumentNumber { get; init; } = default!;
    }
}
