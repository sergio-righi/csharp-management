using Infrastructure.DependencyInjection.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class ResolveOrm
    {
        public static void InfrastructureORM<T>(this IServiceCollection services, IConfiguration configuration = null) where T : IOrm, new()
        {
            var ormType = new T();
            ormType.ResolveOrm(services, configuration);
        }
    }
}
