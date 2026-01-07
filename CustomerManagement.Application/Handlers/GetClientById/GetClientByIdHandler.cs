using CustomerManagement.Application.Queries.GetClientById;
using CustomerManagement.Application.Queries.GetClientById.DTO;
using CustomerManagement.Domain.Interface.Repositories;

namespace CustomerManagement.Application.Handlers.GetClientById
{
    public class GetClientByIdHadler : IGetClientByIdHandler
    {
        private readonly IClientRepository _repository;

        public GetClientByIdHadler(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClientResultDTO?> HandleAsync(GetClientByIdQuery query)
        {
            var cliente = await _repository.GetByIdAsync(query.Id);

            if (cliente is null)
                return null;

            return new ClientResultDTO
            {
                Id = cliente.Id,
                Name = cliente.Name,
                Document = cliente.DocumentNumber.Value,
                Active = cliente.Active
            };

        }
    }
}
