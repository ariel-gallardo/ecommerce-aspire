using Common.Contracts;

namespace Common.Domain.DTOS.Base.Entities
{
    public class Response : IResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class Response<T> : Response, IResponse<T> where T : class
    {
        public T Data { get; set; }
    }
}
