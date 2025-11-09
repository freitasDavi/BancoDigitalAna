namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Auth
{
    public interface IJwtService
    {
        string GenerateToken(string contaId, string numeroConta, string cpf);
        TokenValidationResult Validation(string token);
    }
}
