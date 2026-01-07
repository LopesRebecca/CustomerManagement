using CustomerManagement.Application.Queries.GetClientById;
using CustomerManagement.Application.Queries.GetCustomerResulById.DTO;
using CustomerManagement.Domain.Interface.Repositories;

namespace CustomerManagement.Application.Handlers.GetCustomerById
{
    public class GetCustomerByIdHandler : IGetCustomerByIdHandler
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByIdHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerResultDTO?> HandleAsync(GetCustomerByIdQuery query)
        {
            var cliente = await _repository.GetByIdAsync(query.Id);

            if (cliente is null)
                return null;

            return new CustomerResultDTO
            {
                Id = cliente.Id,
                Name = cliente.Name,
                Document = cliente.DocumentNumber.Value,
                Active = cliente.Active
            };

        }
    }
}
