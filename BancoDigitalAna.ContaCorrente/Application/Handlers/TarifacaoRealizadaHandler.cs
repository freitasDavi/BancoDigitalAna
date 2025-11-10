using BancoDigitalAna.BuildingBlocks.Kafka;
using BancoDigitalAna.Conta.Application.Commands;
using KafkaFlow;
using MediatR;
using System.Drawing;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class TarifacaoRealizadaHandler : IMessageHandler<TarifacaoRealizadaMessage>
    {
        private readonly ILogger<TarifacaoRealizadaHandler> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TarifacaoRealizadaHandler(ILogger<TarifacaoRealizadaHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task Handle(IMessageContext context, TarifacaoRealizadaMessage message)
        {
            try
            {
                _logger.LogInformation(
                        $"[INFO]: Processando tarifação para conta {message.ContaCorrenteId} no valor de {message.ValorTarifado}");

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var command = new NovaMovimentacaoContaCorrenteCommand(null, message.ValorTarifado, 'D', message.ContaCorrenteId);

                await mediator.Send(command);

                _logger.LogInformation(
                        $"[INFO]: Tarifa de {message.ValorTarifado} debitada com sucesso da conta {message.ContaCorrenteId}");
            } catch (Exception ex)
            {
                _logger.LogError($"[ERROR]: Erro ao processar tarifação para conta {message.ContaCorrenteId} ", ex);

                throw;
            }
        }
    }
}
