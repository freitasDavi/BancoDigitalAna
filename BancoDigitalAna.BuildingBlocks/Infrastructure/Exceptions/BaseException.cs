using Microsoft.AspNetCore.Http;

namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Exceptions
{
    public abstract class BaseException : Exception
    {
        public string ErrorCode { get; private set; }
        public int StatusCode { get; protected set; } = StatusCodes.Status400BadRequest;

        public BaseException(string message) : base(message)
        {

        }

        public BaseException(string message, string code) : base(message) 
        {
            ErrorCode = code; 
        }
        
    }
}
