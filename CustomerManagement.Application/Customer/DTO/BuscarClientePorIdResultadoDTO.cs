namespace CustomerManagement.Application.Customer.DTO
{
    public class BuscarClientePorIdResultadoDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Document { get; set; }
        public bool Ativo { get; set; }

        public BuscarClientePorIdResultadoDTO(int id, string name, string document, bool estarAtivo)
        {
            Id = id;
            Name = name;
            Document = document;
            Ativo = estarAtivo;
        }

        public BuscarClientePorIdResultadoDTO()
        {
        }
    }
}
