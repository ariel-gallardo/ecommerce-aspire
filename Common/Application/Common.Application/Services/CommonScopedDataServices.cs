using Common.Application.Contracts;
using System.Reflection;

namespace Common.Application.Services
{
    public class CommonScopedDataServices : ICommonScopedDataServices
    {
        private readonly IList<MethodInfo> processedMethods = new List<MethodInfo>();
        public IList<MethodInfo> ProcessedMethods => processedMethods;
    }
}
