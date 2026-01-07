namespace CustomerManagement.Application.Customer.Interface
{
    public interface ICommandHandler<TCommand, TResult>
    {
         Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}
