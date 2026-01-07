namespace CustomerManagement.Api.Contracts.Response
{
    public class CustomerResponse
    {
        public bool Success { get; init; }
        public string? Error { get; init; } = default!;
        public int? Id { get; init; }
        public string? Name { get; init; } = default!;
        public string? Document { get; init; } = default!;
        public bool? IsActive { get; init; }
    }
}
