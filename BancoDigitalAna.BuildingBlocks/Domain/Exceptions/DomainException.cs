using BancoDigitalAna.BuildingBlocks.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BancoDigitalAna.BuildingBlocks.Domain.Exceptions
{
    public class DomainException : BaseException
    {
        public new string ErrorCode { get; set; }      

        public DomainException(string message, string errorCode, int? statusCode = default) : base(message, errorCode) 
        {
            ErrorCode = errorCode;
            StatusCode = statusCode ?? StatusCodes.Status400BadRequest;
        }
    }
}
