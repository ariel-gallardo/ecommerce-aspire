namespace Common.Infrastructure.Entities.Contracts
{
    public interface IResponse<T> where T : class
    {
        T Data { get; set; }
    }

    public interface IResponse
    {
        string Message { get; set; }
        int StatusCode { get; set; }
    }
}
