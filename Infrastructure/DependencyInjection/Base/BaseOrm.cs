using Infrastructure.DependencyInjection.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection.Base
{
    public abstract class BaseOrm : IOrm
    {
        internal abstract IServiceCollection AddOrm(IServiceCollection services, IConfiguration configuration = null);

        public IServiceCollection ResolveOrm(IServiceCollection services, IConfiguration configuration = null)
        {
            return AddOrm(services, configuration);
        }
    }
}
