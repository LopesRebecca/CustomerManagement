using CustomerManagement.Application.Commands.Request;

namespace CustomerManagement.Api.Extensions
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateClientRequestCommand>();

            return services;
        }
    }
}