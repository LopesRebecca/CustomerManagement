namespace CustomerManagement.Application.Mediator
{
    /// <summary>
    /// Interface do Mediator - centraliza o envio de requests para seus handlers
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Envia um request para seu handler correspondente
        /// </summary>
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
