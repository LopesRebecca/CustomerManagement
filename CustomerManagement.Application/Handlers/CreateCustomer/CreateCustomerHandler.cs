using CustomerManagement.Application.Commands.Request;
using CustomerManagement.Application.Commands.Response;
using CustomerManagement.Application.Handlers.CreateCustomer;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Application.Handlers.Customer
{
    public class CreateCustomerHandler : ICreateCustomerHandler
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateCustomerResponse> HandleAsync(
            CreateCustomerRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var document = DocumentNumber.Create(command.DocumentNumber);

                if (await _repository.ExistDocumentNumberAsync(document, cancellationToken))
                    return CreateCustomerResponse.Failed("Documento já cadastrado.");

                var client = new CustomerEntity(
                    command.Name,
                    document
                );

                await _repository.CreateAsync(client, cancellationToken);

                return CreateCustomerResponse.Ok(client.Id);
            }
            catch (DomainException ex)
            {
                return CreateCustomerResponse.Failed(ex.Message);
            }
        }
    }
}
