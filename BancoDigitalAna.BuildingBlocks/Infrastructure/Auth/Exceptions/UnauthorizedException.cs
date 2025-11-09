namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Auth.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public string ErrorCode { get; private set; } = "USER_UNAUTHORIZED";

        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
