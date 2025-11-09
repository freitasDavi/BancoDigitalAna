using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Auth
{
    public class LoggedUser(IHttpContextAccessor _httpContextAccessor) : ILoggedUser
    {

        public string? ContaId =>
            _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
            _httpContextAccessor.HttpContext?.User
                .FindFirst("sub")?.Value;

        public string? NumeroConta =>
            _httpContextAccessor.HttpContext?.User
                .FindFirst("numeroConta")?.Value;

        public string? Cpf =>
            _httpContextAccessor.HttpContext?.User
                .FindFirst("cpf")?.Value;

        public bool IsAuhtenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
