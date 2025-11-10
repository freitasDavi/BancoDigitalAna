using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Http;
using BancoDigitalAna.Transferencia.Domain.Repositories;
using BancoDigitalAna.Transferencia.Infrastructure.ApiClient;
using BancoDigitalAna.Transferencia.Infrastructure.Repositories;

namespace BancoDigitalAna.Transferencia.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILoggedUser, LoggedUser>();
            services.AddScoped<ITransferenciaRepository, TransferenciaRepository>();
            services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();
            services.AddScoped<IAuthenticatedHttpClient, AuthenticatedHttpClient>();
            services.AddScoped<IContaCorrenteApiClient, ContaCorrenteApiClient>();
            
            return services;
        }
    }
}
