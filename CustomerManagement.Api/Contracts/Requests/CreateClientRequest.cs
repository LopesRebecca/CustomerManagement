namespace CustomerManagement.Api.Contracts.Requests
{
    public class CreateClientRequest
    {
        public string Name { get; init; } = default!;
        public string Document { get; init; } = default!;
    }
}
