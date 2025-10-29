namespace Common.Infrastructure.Entities
{
    public class BaseResponse : IResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class Response<T> : BaseResponse, IResponse<T> where T : class
    {
        public T Data { get; set; }
    }
}
