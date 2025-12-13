namespace Common.Contracts.Queries
{
    public interface IQueryModifier<T> : IScoped where T : class
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }

}
