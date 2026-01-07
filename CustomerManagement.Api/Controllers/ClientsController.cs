using CustomerManagement.Api.Contracts.Requests;
using CustomerManagement.Application.Commands.Request;
using CustomerManagement.Application.Handlers.CreateClient;
using CustomerManagement.Application.Handlers.GetClientById;
using CustomerManagement.Application.Queries.GetClientById;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public sealed class ClientsController : ControllerBase
    {
        private readonly ICreateClientHandler _createClientHandler;
        private readonly IGetClientByIdHadler _getClientByIdHadler;

        public ClientsController(
            ICreateClientHandler createClientHandler,
            IGetClientByIdHadler getClientByIdHadler)
        {
            _createClientHandler = createClientHandler;
            _getClientByIdHadler = getClientByIdHadler;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] CreateClientRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateClientRequestCommand
            {
                Name = request.Name,
                DocumentNumber = request.Document
            };

            var result = await _createClientHandler.HandleAsync(command, cancellationToken);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(result);
        }

        [HttpGet("${id}")]
        public async Task<IActionResult> Get(int idClient)
        {
            var query = new GetClientByIdQuery
            {
                Id = idClient
            };

            var result = await _getClientByIdHadler.HandleAsync(query);

            return Ok(result);
        }
    }
}
