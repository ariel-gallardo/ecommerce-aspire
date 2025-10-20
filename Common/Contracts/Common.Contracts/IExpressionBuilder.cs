using System.Linq.Expressions;

namespace Common.Contracts
{
    public interface IExpressionBuilder
    {
        Expression<Func<T, bool>> Build<T>(object filter) where T : class;
    }
}
