using CustomerManagement.Api.Contracts.Requests;
using CustomerManagement.Application.Customer.Commands;
using CustomerManagement.Application.Customer.Queries;
using CustomerManagement.Application.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public sealed class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] CreateCustomerRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateCustomerCommand
            {
                Name = request.Name,
                DocumentNumber = request.Document
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Sucess)
                return BadRequest(new { error = result.Message });

            return Ok(result);
        }

        [HttpGet("{idClient}")]
        public async Task<IActionResult> Get(int idClient, CancellationToken cancellationToken = default)
        {
            var query = new GetCustomerByIdQuery
            {
                Id = idClient
            };

            var result = await _mediator.Send(query, cancellationToken);

            if(result is null)
                return NotFound("Cliente com o Id informado não encontrado");

            return Ok(result);
        }
    }
}
