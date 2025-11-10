namespace BancoDigitalAna.Transferencia.Application.DTOs
{
    public record ConsultaContaPorNumeroResponse (Guid IdConta, string NomeTitular, string Cpf)
    {
    }
}
