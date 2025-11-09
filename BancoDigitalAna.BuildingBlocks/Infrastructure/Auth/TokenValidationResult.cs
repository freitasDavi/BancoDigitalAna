namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Auth
{
    public class TokenValidationResult
    {
        public bool IsValid { get; set; }
        public string? ContaId { get; set; }
        public string? NumeroConta {  get; set; }
        public string? Cpf {  get; set; }
        public string? ErrorMessage { get; set; }
    }
}
