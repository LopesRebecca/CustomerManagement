using CustomerManagement.Application.Handlers.CreateClient;

namespace CustomerManagement.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateClientHandler, CreateClientHandler>();

            return services;
        }
    }
}