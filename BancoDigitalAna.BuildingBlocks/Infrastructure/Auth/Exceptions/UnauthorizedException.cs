using BancoDigitalAna.BuildingBlocks.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Auth.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public new string ErrorCode { get; private set; } = "USER_UNAUTHORIZED";
        public new int StatusCode { get; private set; } = StatusCodes.Status403Forbidden;

        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
