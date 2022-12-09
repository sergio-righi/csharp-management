using Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using Tool.Configurations;

namespace Infrastructure.DependencyInjection
{
    internal class ResolveConfiguration
    {
        public static IConfiguration GetConnectionSettings(IConfiguration configuration)
        {
            return configuration ?? DatabaseConnection.ConnectionConfiguration;
        }
    }
}
