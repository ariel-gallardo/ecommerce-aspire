namespace Common.Infrastructure.Contracts
{
    public interface IQueryModifier<T> : IScoped where T : class
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }

}
