using Common.Infrastructure.Entities.Contracts;

namespace Common.Contracts.Behavior.Base
{
    public interface IServiceBehavior<TRequest>
    {
        int Order { get; }
    }
    public interface IServiceBehavior<TRequest, TResponse> : IServiceBehavior<TRequest>
    {
    }
}
