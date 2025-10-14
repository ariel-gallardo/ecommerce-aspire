namespace Common.Domain.DTOS.Base.Entities
{
    public class Response
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class Response<T> : Response where T : class
    {
        public T Data { get; set; }
    }
}
