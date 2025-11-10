
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Http
{
    public class AuthenticatedHttpClient(HttpClient _httpClient, IHttpContextAccessor _httpContextAccessor) : IAuthenticatedHttpClient
    {
        public async Task<HttpResponseMessage> GetAsync(string requestUri, string? token = null)
        {
            ConfigureAuthentication(token);

            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content, string? token = null)
        {
            ConfigureAuthentication(token);

            return await _httpClient.PostAsJsonAsync<T>(requestUri, content);
        }

        private void ConfigureAuthentication(string? explicitToken)
        {
            var token = explicitToken ?? GetCurrentToken();

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private string? GetCurrentToken ()
        {
            var authHeader = _httpContextAccessor.HttpContext?
                .Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return null;

            return authHeader.Substring("Bearer ".Length).Trim();
        }
    }
}
