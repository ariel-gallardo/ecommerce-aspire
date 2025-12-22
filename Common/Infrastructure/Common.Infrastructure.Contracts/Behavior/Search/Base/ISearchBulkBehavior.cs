namespace Common.Infrastructure.Contracts
{
    public interface ISearchBulkBehavior<ResultDTO> : IServiceBehavior<IQuerieFilter, ResultDTO> where ResultDTO : class, IEntityDTO
    {

    }
}
