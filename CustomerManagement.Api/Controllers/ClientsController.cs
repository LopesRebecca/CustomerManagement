using CustomerManagement.Api.Contracts.Requests;
using CustomerManagement.Application.Commands.Request;
using CustomerManagement.Application.Handlers.CreateClient;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public sealed class ClientsController : ControllerBase
    {
        private readonly ICreateClientHandler _createClientHandler;

        public ClientsController(ICreateClientHandler createClientHandler)
        {
            _createClientHandler = createClientHandler;
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

            return Created($"api/clientes/{result.ClientId}", result);
        }
    }
}
