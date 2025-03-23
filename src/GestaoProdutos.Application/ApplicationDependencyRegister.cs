using GestaoProdutos.Application.Factories;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Application.Services;
using GestaoProdutos.Application.Strategies;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace GestaoProdutos.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationDependencyRegister
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDependencyServices();
        }

        private static IServiceCollection AddDependencyServices(this IServiceCollection services)
        {
            services.AddTransient<ICalculateTaxFactory, CalculateTaxFactory>();
            services.AddTransient<ICalculateTaxStrategy, CalculateTaxDefault>();
            services.AddTransient<ICalculateTaxStrategy, CalculateTaxReform>();
            services.AddTransient<IOrderService, OrderService>();

            return services;
        }
    }
}
