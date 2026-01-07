namespace CustomerManagement.Api.Contracts.Response
{
    public class CreateClientResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string Document { get; init; } = default!;
        public bool IsActive { get; init; }
    }
}
