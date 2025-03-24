using GestaoProdutos.Infra.Context;
using GestaoProdutos.Infra.Interfaces;
using GestaoProdutos.InfraEstructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace GestaoProdutos.Infra
{
    [ExcludeFromCodeCoverage]
    public static class InfraDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencyContext(configuration);
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("GestaoProdutosConnection");
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
