
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;
using System.Text.Json;

namespace BancoDigitalAna.Conta.Application.Services
{
    public class IdempotenciaService(IIdempotenciaRepository _repository, ILogger<IIdempotenciaService> _logger) : IIdempotenciaService
    {
        public async Task<T?> ExecutarComIdempotenciaAsync<T>(Guid chaveIdempotencia, object requisicao, Func<Task<T>> operacao)
        {
            var resultadoExistente = await _repository.RecuperarIdempotencia(chaveIdempotencia);

            if (!string.IsNullOrEmpty(resultadoExistente))
            {
                _logger.LogInformation($"[INFO]: Requisição idempotente detectada: {chaveIdempotencia}");

                return JsonSerializer.Deserialize<T>(resultadoExistente);
            }

            var resultado = await operacao();

            try
            {
                var requisicaoJson = JsonSerializer.Serialize(requisicao);
                var resultadoJson = JsonSerializer.Serialize(resultado);

                var idempotencia = Idempotencia.Criar(chaveIdempotencia, requisicaoJson, resultadoJson);

                await _repository.SalvarAsync(idempotencia);

                _logger.LogInformation($"[INFO]: Resultado da operação salvo para chave: {chaveIdempotencia}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERROR]: Erro ao salvar idempotência para chave {chaveIdempotencia} " + ex);
            }

            return resultado;
        }
    }
    
}
