using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Customer.Queries;
using CustomerManagement.Application.Mediator;
using CustomerManagement.Domain.Interface.Repositories;

namespace CustomerManagement.Application.Customer.Handlers
{
    public class BuscarClientePorIdQueryHandler : IRequestHandler<BuscarClientePorIdQuery, BuscarClientePorIdResultadoDTO?>
    {
        private readonly IClienteRepository _repositorio;

        public BuscarClientePorIdQueryHandler(IClienteRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<BuscarClientePorIdResultadoDTO?> Handle(
            BuscarClientePorIdQuery consulta, 
            CancellationToken cancellationToken = default)
        {
            var cliente = await _repositorio.ObterPorIdAsync(consulta.Id);

            if (cliente is null)
                return null;

            return new BuscarClientePorIdResultadoDTO
            {
                Id = cliente.Id,
                Name = cliente.Nome,
                Document = cliente.NumeroDocumento.ToString(),
                Ativo = cliente.Ativo
            };
        }
    }
}
