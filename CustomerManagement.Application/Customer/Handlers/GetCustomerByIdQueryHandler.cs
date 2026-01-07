using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Customer.Queries;
using CustomerManagement.Application.Mediator;
using CustomerManagement.Domain.Interface.Repositories;

namespace CustomerManagement.Application.Customer.Handlers
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, GetCustomerByIdResultDTO?>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByIdQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetCustomerByIdResultDTO?> Handle(
            GetCustomerByIdQuery query, 
            CancellationToken cancellationToken = default)
        {
            var customer = await _repository.GetByIdAsync(query.Id);

            if (customer is null)
                return null;

            return new GetCustomerByIdResultDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                Document = customer.DocumentNumber.ToString(),
                IsActive = customer.Active
            };
        }
    }
}
