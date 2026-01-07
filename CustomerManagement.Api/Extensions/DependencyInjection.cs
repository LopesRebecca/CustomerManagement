using CustomerManagement.Application.Customer.Handlers;
using CustomerManagement.Application.Mediator;

namespace CustomerManagement.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registra o Mediator e todos os handlers do assembly Application
            services.AddMediator(typeof(CreateCustomerCommandHandler).Assembly);

            return services;
        }
    }
}