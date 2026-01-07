using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Application.Mediator
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra o Mediator e todos os handlers encontrados no assembly especificado
        /// </summary>
        public static IServiceCollection AddMediator(
            this IServiceCollection services, 
            params Assembly[] assemblies)
        {
            // Registra o Mediator
            services.AddScoped<IMediator, Mediator>();

            // Se nenhum assembly foi passado, usa o assembly que chamou
            if (assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            // Registra todos os handlers automaticamente
            foreach (var assembly in assemblies)
            {
                RegisterHandlersFromAssembly(services, assembly);
            }

            return services;
        }

        private static void RegisterHandlersFromAssembly(IServiceCollection services, Assembly assembly)
        {
            var handlerInterfaceType = typeof(IRequestHandler<,>);

            var handlerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetInterfaces().Any(i => 
                    i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType));

            foreach (var handlerType in handlerTypes)
            {
                var implementedInterfaces = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);

                foreach (var interfaceType in implementedInterfaces)
                {
                    services.AddScoped(interfaceType, handlerType);
                }
            }
        }
    }
}
