namespace CustomerManagement.Application.Commands.Response
{
    public class CreateClientResponse
    {
        public bool Sucess { get; }
        public string? Erro { get; }
        public int? ClientId { get; }

        private CreateClientResponse(bool sucesso, string? erro, int? clientId)
        {
            Sucess = sucesso;
            Erro = erro;
            ClientId = clientId;
        }

        public static CreateClientResponse Ok(int id)
            => new(true, null, id);

        public static CreateClientResponse Failed(string erro)
            => new(false, erro, null);
    }
}
