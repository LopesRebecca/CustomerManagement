namespace CustomerManagement.Application.Customer.DTO
{
    public class CriarClienteResultadoDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public int? ClienteId { get; set; }

        public CriarClienteResultadoDTO(bool sucesso, string mensagem, int? clienteId)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            ClienteId = clienteId;
        }
        public static CriarClienteResultadoDTO Ok(int id)
            => new(true, "Cadastro realizado com sucesso!", id);

        public static CriarClienteResultadoDTO Falha(string error)
            => new(false, error, null);
    }
}
