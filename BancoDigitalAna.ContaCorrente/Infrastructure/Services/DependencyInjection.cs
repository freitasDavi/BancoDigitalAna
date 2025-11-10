using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.Conta.Application.Services;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Database;
using BancoDigitalAna.Conta.Infrastructure.Repositories;

namespace BancoDigitalAna.Conta.Infrastructure.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services) 
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILoggedUser, LoggedUser>();
            services.AddScoped<IContaRepository, ContaRepository>();
            services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();
            services.AddScoped<IIdempotenciaService, IdempotenciaService>();

            return services;
        }
    }
}
