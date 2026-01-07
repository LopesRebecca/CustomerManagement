using CustomerManagement.Application.Commands.Request;
using CustomerManagement.Application.Commands.Response;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Application.Handlers.CreateClient
{
    public class CreateClientHandler : ICreateClientHandler
    {
        private readonly IClientRepository _repository;

        public CreateClientHandler(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateClientResponse> HandleAsync(
            CreateClientRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var documento = DocumentNumber.Create(command.DocumentNumber);

                if (await _repository.ExistDocumentNumberAsync(documento, cancellationToken))
                    return CreateClientResponse.Failed("Documento já cadastrado.");

                var cliente = new ClientEntity(
                    command.Name,
                    documento
                );

                await _repository.CreateAsync(cliente, cancellationToken);

                return CreateClientResponse.Ok(cliente.Id);
            }
            catch (DomainException ex)
            {
                return CreateClientResponse.Failed(ex.Message);
            }
        }
    }
}
