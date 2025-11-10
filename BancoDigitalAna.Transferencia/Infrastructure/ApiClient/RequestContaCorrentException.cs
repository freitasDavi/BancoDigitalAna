using BancoDigitalAna.BuildingBlocks.Infrastructure.Exceptions;

namespace BancoDigitalAna.Transferencia.Infrastructure.ApiClient
{
    public class RequestContaCorrentException : BaseException
    {
        public new int StatusCode { get; private set; } = StatusCodes.Status400BadRequest;
        public RequestContaCorrentException(string message, string errorCode) : base(message, errorCode)
        {
        }
    }
}
