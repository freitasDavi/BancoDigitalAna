namespace BancoDigitalAna.Tarifacao.Messages
{
    public record TransferenciaRealizadaMessage (Guid IdRequisicao, Guid ContaCorrenteId, decimal ValorTransferencia, DateTime DataHora)
    {

    }
}
