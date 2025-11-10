
using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Http;
using BancoDigitalAna.Transferencia.Application.DTOs;
using System.Text.Json;

namespace BancoDigitalAna.Transferencia.Infrastructure.ApiClient
{
    public class ContaCorrenteApiClient : IContaCorrenteApiClient
    {

        private readonly string baseUrl;
        private readonly IAuthenticatedHttpClient _httpClient;
        private readonly ILogger<ContaCorrenteApiClient> _logger;

        public ContaCorrenteApiClient(
            IAuthenticatedHttpClient httpClient,
            ILogger<ContaCorrenteApiClient> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            baseUrl = configuration.GetValue<string>("ApiContaCorrenteUrl")!;
        }

        public async Task<ConsultaContaPorNumeroResponse?> ConsultaContaPorNumero(string numeroConta)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{baseUrl}Conta/{numeroConta}");

                if (response.IsSuccessStatusCode)
                {
                    var conta = await response.Content.ReadFromJsonAsync<ConsultaContaPorNumeroResponse>();

                    return conta;
                }

                throw new RequestContaCorrentException("Apenas contas correntes cadastradas podem realizar transferências", "INVALID_ACCOUNT");

            } catch (Exception ex)
            {
                _logger.LogError("[ERROR]: Erro ao Consultar API ContaCorrente para recuperar conta para movimento: " + ex.Message);
                throw new RequestContaCorrentException("Erro ao recuperar conta para movimento", "INVALID_ACCOUNT");
            }
        }

        public async Task RealizarMovimentoAsync(NovoMovimentoRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsync<NovoMovimentoRequest>($"{baseUrl}Conta/movimento", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var error = JsonSerializer.Deserialize<ErrorResponse>(
                            errorContent,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        throw new RequestContaCorrentException(
                            error?.Mensagem ?? $"Erro ao realizar movimento: {response.StatusCode}",
                            error?.Tipo ?? "MOVIMENTO_ERROR"
                         );

                    } catch (JsonException ex)
                    {
                        throw new RequestContaCorrentException(
                            $"Erro ao realizar movimento: {response.StatusCode}",
                            "MOVIMENTO_ERROR"
                         );
                    }
                }

            } 
            catch (RequestContaCorrentException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERROR]: Erro ao realizar requisição para API de movimentos ", ex.Message);

                throw new RequestContaCorrentException(
                            "Erro de comunicação com API de Conta Corrente",
                            "API_COMMUNICATION_ERROR"
                         );
            }
        }

        public async Task<bool> ValidarContaAsync(Guid contaId)
        {
            try
            {
                var response = await _httpClient.GetAsync("contas/consulta");

                _logger.LogInformation("[INFO]: Consultando saldo da Conta");

                return response.IsSuccessStatusCode;
            } catch (Exception ex)
            {
                _logger.LogError($"[ERROR]: Erro ao validar conta {contaId}: " + ex);

                return false;
            }
        }
    }
}
