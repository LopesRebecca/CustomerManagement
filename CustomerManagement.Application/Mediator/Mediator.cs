using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Application.Mediator
{
    /// <summary>
    /// Implementação do Mediator que resolve handlers via DI
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request, 
            CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            // Monta o tipo do handler: IRequestHandler<TRequest, TResponse>
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            // Resolve o handler do container de DI
            var handler = _serviceProvider.GetService(handlerType);

            if (handler is null)
            {
                throw new InvalidOperationException(
                    $"Nenhum handler registrado para '{requestType.Name}'. " +
                    $"Certifique-se de registrar IRequestHandler<{requestType.Name}, {responseType.Name}> no container de DI.");
            }

            // Invoca o método Handle via reflection
            var handleMethod = handlerType.GetMethod("Handle");
            
            if (handleMethod is null)
            {
                throw new InvalidOperationException(
                    $"Método 'Handle' não encontrado no handler para '{requestType.Name}'.");
            }

            var result = handleMethod.Invoke(handler, new object[] { request, cancellationToken });

            if (result is Task<TResponse> task)
            {
                return await task;
            }

            throw new InvalidOperationException(
                $"O handler para '{requestType.Name}' não retornou um Task<{responseType.Name}>.");
        }
    }
}
