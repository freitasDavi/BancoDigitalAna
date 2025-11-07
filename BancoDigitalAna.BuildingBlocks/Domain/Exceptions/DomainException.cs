namespace BancoDigitalAna.BuildingBlocks.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; set; }

        public DomainException(string message, string errorCode) : base(message) 
        {
            ErrorCode = errorCode;
        }
    }
}
