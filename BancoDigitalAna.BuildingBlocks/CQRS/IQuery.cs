using MediatR;

namespace BancoDigitalAna.BuildingBlocks.CQRS
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
        where TResponse : notnull
    {
    }
}
