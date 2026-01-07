namespace CustomerManagement.Application.Customer.Interface
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult?> HandleAsync(TQuery query);
    }
}
