
namespace Common.Infrastructure.Contracts
{
    public interface ISearchSingleBehavior<TRequest, ResultDTO> : IServiceBehavior<TRequest, ResultDTO> where ResultDTO : class, IEntityDTO
    {
  
    }
}
