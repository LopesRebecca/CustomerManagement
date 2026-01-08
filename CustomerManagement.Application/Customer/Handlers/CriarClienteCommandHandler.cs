using CustomerManagement.Application.Customer.Commands;
using CustomerManagement.Application.Customer.DTO;
using CustomerManagement.Application.Mediator;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.Interface.Repositories;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Application.Customer.Handlers
{
    public class CriarClienteCommandHandler : IRequestHandler<CriarClienteCommand, CriarClienteResultadoDTO>
    {
        private readonly IClienteRepository _repositorio;

        public CriarClienteCommandHandler(IClienteRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<CriarClienteResultadoDTO> Handle(
            CriarClienteCommand comando, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var documento = NumeroDocumento.Create(comando.NumeroDocumento);

                if (await _repositorio.ExisteNumeroDocumentoAsync(documento, cancellationToken))
                    return CriarClienteResultadoDTO.Falha("Documento já cadastrado.");

                var cliente = new Cliente(
                    comando.Nome,
                    documento
                );

                await _repositorio.CriarAsync(cliente, cancellationToken);

                return CriarClienteResultadoDTO.Ok(cliente.Id);
            }
            catch (DomainException ex)
            {
                return CriarClienteResultadoDTO.Falha(ex.Message);
            }
        }
    }
}
