namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Http
{
    public interface IAuthenticatedHttpClient
    {
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T httpContent, string? token = null);
        Task<HttpResponseMessage> GetAsync(string requestUri, string? token = null);
    }
}
