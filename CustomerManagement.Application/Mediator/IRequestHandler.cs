namespace CustomerManagement.Application.Mediator
{
    /// <summary>
    /// Interface para handlers que processam um request e retornam uma resposta
    /// </summary>
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface para handlers que processam um request sem retorno
    /// </summary>
    public interface IRequestHandler<TRequest> : IRequestHandler<TRequest, Unit>
        where TRequest : IRequest<Unit>
    {
    }
}
