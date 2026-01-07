namespace Common.Infrastructure.Contracts
{
    public interface IResponse<T> : IResponse where T : class
    {
        T Data { get; set; }
    }

    public interface IResponse
    {
        string Message { get; set; }
        int StatusCode { get; set; }
    }
}
