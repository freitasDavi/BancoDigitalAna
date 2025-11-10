namespace BancoDigitalAna.Conta.Application.DTOs
{
    public record ConsultaSaldoResponse(string numeroConta, string titular, DateTime horaConsulta, decimal saldoAtual)
    {
    }
}
