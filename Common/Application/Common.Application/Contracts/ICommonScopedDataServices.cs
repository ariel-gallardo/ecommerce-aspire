using Common.Contracts;
using System.Reflection;

namespace Common.Application.Contracts
{
    public interface ICommonScopedDataServices : IScoped
    {
        IList<MethodInfo> ProcessedMethods { get; }
    }
}
