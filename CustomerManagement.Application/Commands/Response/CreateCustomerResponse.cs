namespace CustomerManagement.Application.Commands.Response
{
    public class CreateCustomerResponse
    {
        public bool Success { get; }
        public string? Error { get; }
        public int? ClientId { get; }

        private CreateCustomerResponse(bool success, string? error, int? clientId)
        {
            Success = success;
            Error = error;
            ClientId = clientId;
        }

        public static CreateCustomerResponse Ok(int id)
            => new(true, null, id);

        public static CreateCustomerResponse Failed(string error)
            => new(false, error, null);
    }
}
