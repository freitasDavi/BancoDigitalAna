using BancoDigitalAna.BuildingBlocks.Infrastructure.Exceptions;

namespace BancoDigitalAna.Conta.Infrastructure.Exceptions
{
    public class ContaCorrenteException : BaseException
    {
        public ContaCorrenteException(string message, string code) : base(message, code)
        {
        }
    }
}
