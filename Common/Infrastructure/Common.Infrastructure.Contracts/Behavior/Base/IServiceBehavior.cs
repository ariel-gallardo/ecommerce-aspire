namespace Common.Infrastructure.Contracts
{
    public interface IServiceBehavior<TRequest> : ITransient
    {
        int Order { get; }
    }
    public interface IServiceBehavior<TRequest, TResponse> : IServiceBehavior<TRequest>
    {
    }
}
