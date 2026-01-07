using CustomerManagement.Application.Customer.Commands;
using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Mediator;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Application.Customer.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResultDTO>
    {
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateCustomerResultDTO> Handle(
            CreateCustomerCommand command, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var document = DocumentNumber.Create(command.DocumentNumber);

                if (await _repository.ExistDocumentNumberAsync(document, cancellationToken))
                    return CreateCustomerResultDTO.Failed("Documento já cadastrado.");

                var client = new CustomerEntity(
                    command.Name,
                    document
                );

                await _repository.CreateAsync(client, cancellationToken);

                return CreateCustomerResultDTO.Ok(client.Id);
            }
            catch (DomainException ex)
            {
                return CreateCustomerResultDTO.Failed(ex.Message);
            }
        }
    }
}
