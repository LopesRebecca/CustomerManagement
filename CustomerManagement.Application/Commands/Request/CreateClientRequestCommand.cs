namespace CustomerManagement.Application.Commands.Request
{
    public class CreateClientRequestCommand
    {
        public required string Name { get; init; }
        public required string DocumentNumber { get; init; }
    }
}
