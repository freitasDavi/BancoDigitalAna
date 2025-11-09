using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Database;
using BancoDigitalAna.Conta.Infrastructure.Repositories;

namespace BancoDigitalAna.Conta.Infrastructure.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services) 
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IContaRepository, ContaRepository>();

            return services;
        }
    }
}
