namespace Common.Domain.Exceptions
{
    public class CacheNotFoundException : Exception
    {
        public CacheNotFoundException(string key, int times) : base($"The following dependencies are not ready in cache (Key,Times): [{key}|{times}]")
        {
            
        }
    }
}
