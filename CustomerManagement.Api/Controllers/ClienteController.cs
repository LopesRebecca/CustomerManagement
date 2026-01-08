using CustomerManagement.Api.Contracts.Requests;
using CustomerManagement.Application.Customer.Commands;
using CustomerManagement.Application.Customer.Queries;
using CustomerManagement.Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public sealed class ClienteController : ControllerBase
    {
        private readonly IMediator _mediador;

        public ClienteController(IMediator mediador)
        {
            _mediador = mediador;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] CriarClienteRequest requisicao,
            CancellationToken cancellationToken = default)
        {
            var comando = new CriarClienteCommand
            {
                Nome = requisicao.Nome,
                NumeroDocumento = requisicao.NumeroDocumento
            };

            var resultado = await _mediador.Send(comando, cancellationToken);

            if (!resultado.Sucesso)
                return BadRequest(new { error = resultado.Mensagem });

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id, CancellationToken cancellationToken = default)
        {
            var consulta = new BuscarClientePorIdQuery
            {
                Id = id
            };

            var resultado = await _mediador.Send(consulta, cancellationToken);

            if(resultado is null)
                return NotFound("Cliente com o Id informado não encontrado");

            return Ok(resultado);
        }
    }
}
