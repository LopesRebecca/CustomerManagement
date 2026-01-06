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
        private readonly CreateClientHandler _createClientHandler;

        public ClientsController(
            CreateClientHandler createClientHandler)
        {
            _createClientHandler = createClientHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CreateClientRequest request)
        {
            var command = new CreateClientRequestCommand
            {
                Name = request.Name,
                DocumentNumber = request.Document
            };

            var result = await _createClientHandler.HandleAsync(command);

            if (!result.Sucess)
                return BadRequest(new { erro = result.Erro });

            return Ok(result);
        }
    }
}
