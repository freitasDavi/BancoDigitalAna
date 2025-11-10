using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.BuildingBlocks.Kafka;
using BancoDigitalAna.Transferencia.Application.Commands;
using BancoDigitalAna.Transferencia.Application.DTOs;
using BancoDigitalAna.Transferencia.Domain.Entities;
using BancoDigitalAna.Transferencia.Domain.Repositories;
using BancoDigitalAna.Transferencia.Infrastructure.ApiClient;
using BancoDigitalAna.Transferencia.Infrastructure.Producers;
using MediatR;
using System.Text.Json;

namespace BancoDigitalAna.Transferencia.Application.Handlers
{
    public class NovaTransferenciaCommandHandler(
        ITransferenciaRepository _transferenciaRepository,
        IIdempotenciaRepository _idempotenciaRepository,
        IContaCorrenteApiClient _apiClient,
        ILogger<NovaTransferenciaCommandHandler> _logger,
        ITransferenciaProducer _transferenciaProducer
        
        ) : ICommandHandler<NovaTransferenciaCommand, TransferenciaResponse>
    {
        public async Task<TransferenciaResponse> Handle(NovaTransferenciaCommand request, CancellationToken cancellationToken)
        {
            if (request.Valor <= 0)
            {
                throw new RequestContaCorrentException("Valor deve ser maior que zero", "INVALID_VALUE");
            }

            var requisicaoJaRealizada = await _idempotenciaRepository.RecuperarIdempotencia(request.ChaveIdempotencia);

            if (!string.IsNullOrEmpty(requisicaoJaRealizada))
            {
                _logger.LogInformation($"[INFO]: Requisição idempotente ja realizada: {request.ChaveIdempotencia}");

                return new TransferenciaResponse(request.ChaveIdempotencia.ToString(), "Transferência já realizada");
            }

            var contaDestino = await _apiClient.ConsultaContaPorNumero(request.ContaDestinoNumero);

            if (contaDestino is null)
                throw new RequestContaCorrentException("Apenas contas correntes cadastradas podem realizar transferências", "INVALID_ACCOUNT");


            var transferencia = Transferencias.Criar(request.ContaOrigemId, contaDestino.IdConta, request.Valor);
            bool debitoRealizado = false;
            bool creditoRealizado = false;

            try
            {
                _logger.LogInformation($"[INFO]: Realizando débito com o valor {request.Valor} na conta de origem {request.ContaOrigemId}");

                await _apiClient.RealizarMovimentoAsync(new NovoMovimentoRequest(request.ContaOrigemId, request.Valor, 'D'));

                debitoRealizado = true;

                _logger.LogInformation($"[INFO]: Realizando crédito com o valor {request.Valor} na conta de destino {contaDestino.IdConta}");

                await _apiClient.RealizarMovimentoAsync(new NovoMovimentoRequest(contaDestino.IdConta, request.Valor, 'C'));

                creditoRealizado = true;

                await _transferenciaRepository.InserirAsync(transferencia);

                var response = new TransferenciaResponse(transferencia.Id.ToString(), "Transferência realizada com sucesso");

                var requisicaoRealizada = JsonSerializer.Serialize(new
                {
                    request.ContaOrigemId,
                    contaDestino.IdConta,
                    request.Valor,
                    timeStamp = DateTime.UtcNow
                });

                var resultadoJson = JsonSerializer.Serialize(response);

                _logger.LogInformation($"[INFO]: Transferência {transferencia.Id.ToString()} realizada com sucesso");

                await _transferenciaProducer.PublicarTransferenciaRealizada(
                    new TransferenciaRealizadaMessage(request.ChaveIdempotencia, request.ContaOrigemId, request.Valor, DataHora: DateTime.UtcNow
                ));

                _logger.LogInformation($"[INFO]: Evento de transferência publicado no Kafka para {request.ChaveIdempotencia}");

                return response;
            } catch (RequestContaCorrentException ex)
            {
                _logger.LogError($"[ERROR]: Erro ao realizar transferências, o estorno será realizado");

                if (creditoRealizado)
                    await RealizaEstorno(contaDestino.IdConta, request.Valor, 'D');

                if (debitoRealizado)
                    await RealizaEstorno(request.ContaOrigemId, request.Valor, 'C');

                throw;
            } catch (Exception ex)
            {
                _logger.LogError("[ERROR]: Erro ao realizar trasnferência " + ex.Message);

                throw;
            }

        }

        private async Task RealizaEstorno(Guid idConta, decimal valor, char tipoEstorno)
        {
            try
            {
                await _apiClient.RealizarMovimentoAsync(new NovoMovimentoRequest(idConta, valor, tipoEstorno));

                _logger.LogInformation("Estorno realizado com sucesso");
            } catch (Exception ex)
            {
                _logger.LogError($"[ERROR]: Erro ao realizar estorno {valor}. Uma nova tentativa será realizada" + ex.Message);

                /// Jogar tarefa no kafka para realizar novamente o estorno
                
                throw;

            }
        }
    }
}
