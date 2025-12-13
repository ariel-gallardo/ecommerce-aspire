using Common.Contracts.Behavior.Base;

namespace Common.Contracts.Behavior.Search.Base
{
    public interface ISearchSingleBehavior<TRequest, ResultDTO> : IServiceBehavior<TRequest, ResultDTO> where ResultDTO : class, DTO.Base.IEntityDTO
    {
  
    }
}
