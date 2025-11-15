namespace Common.Infrastructure.Cache.Key
{
    public static class CacheKeyCommon
    {
        public static string PolicyActionNameCreated(string controllerName, string actionName)
        {
            var actionCache = $"Policy:Created:Action:{controllerName}:{actionName}";
            return actionCache;
        }
        public static string PolicyActionName(string controllerName, string actionName)
        {
            var actionCache = $"Policy:Action:{controllerName}:{actionName}";
            return actionCache;
        }
    }
}
