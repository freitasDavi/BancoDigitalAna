using MediatR;

namespace BancoDigitalAna.BuildingBlocks.CQRS
{
    internal interface IQueryHandler<in TQuery, TResponse>
        : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
