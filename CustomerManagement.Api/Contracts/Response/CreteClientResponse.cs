namespace CustomerManagement.Api.Contracts.Response
{
    public class CreteClientResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string Documents { get; init; } = default!;
        public bool Activate { get; init; }
    }
}
