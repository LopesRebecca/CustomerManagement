using CustomerManagement.Api.Contracts.Requests;
using CustomerManagement.Application.Commands.Request;
using CustomerManagement.Application.Handlers.CreateCustomer;
using CustomerManagement.Application.Handlers.GetCustomerById;
using CustomerManagement.Application.Queries.GetClientById;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public sealed class CustomerController : ControllerBase
    {
        private readonly ICreateCustomerHandler _createClientHandler;
        private readonly IGetCustomerByIdHandler _getClientByIdHadler;

        public CustomerController(
            ICreateCustomerHandler createClientHandler,
            IGetCustomerByIdHandler getClientByIdHandler)
        {
            _createClientHandler = createClientHandler;
            _getClientByIdHadler = getClientByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] CreateCustomerRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateCustomerRequestCommand
            {
                Name = request.Name,
                DocumentNumber = request.Document
            };

            var result = await _createClientHandler.HandleAsync(command, cancellationToken);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(result);
        }

        [HttpGet("{idClient}")]
        public async Task<IActionResult> Get([FromRoute(Name = "id")]int idClient)
        {
            var query = new GetCustomerByIdQuery
            {
                Id = idClient
            };

            var result = await _getClientByIdHadler.HandleAsync(query);

            if(result is null)
                return NotFound("Cliente com o Id informado não encontrado");

            return Ok(result);
        }
    }
}
