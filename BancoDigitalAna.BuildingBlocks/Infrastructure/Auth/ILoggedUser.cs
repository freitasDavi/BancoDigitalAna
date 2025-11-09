namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Auth
{
    public interface ILoggedUser
    {
        string? ContaId { get; }
        string? NumeroConta { get; }
        string? Cpf { get; }
        bool IsAuhtenticated { get; }
    }
}
