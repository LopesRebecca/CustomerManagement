using CustomerManagement.Application.Customer.Handlers;
using CustomerManagement.Application.Mediator;
using ValidationsGeneral.Extensions;

namespace CustomerManagement.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator(typeof(CreateCustomerCommandHandler).Assembly);
            services.AddMediator(typeof(GetCustomerByIdQueryHandler).Assembly);
            services.AddValidationStrategies();

            return services;
        }
    }
}