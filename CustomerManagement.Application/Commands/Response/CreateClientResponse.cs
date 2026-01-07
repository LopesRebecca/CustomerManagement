namespace CustomerManagement.Application.Commands.Response
{
    public class CreateClientResponse
    {
        public bool Success { get; }
        public string? Error { get; }
        public int? ClientId { get; }

        private CreateClientResponse(bool success, string? error, int? clientId)
        {
            Success = success;
            Error = error;
            ClientId = clientId;
        }

        public static CreateClientResponse Ok(int id)
            => new(true, null, id);

        public static CreateClientResponse Failed(string error)
            => new(false, error, null);
    }
}
