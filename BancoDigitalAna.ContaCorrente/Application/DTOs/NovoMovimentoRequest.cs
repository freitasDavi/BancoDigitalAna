using BancoDigitalAna.Conta.Domain.Entities;

namespace BancoDigitalAna.Conta.Application.DTOs
{
    public record NovoMovimentoRequest (string? NumeroConta, decimal Valor, char Tipo)
    {
    }
}
