using Common.Domain.DTOS.Base.Entities;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Api.SwaggerExamples.UserLogin
{
    public class UserLoginResponseOk : IExamplesProvider<Response<string>>
    {
        public Response<string> GetExamples()
        {
            return new Response<string>
            {
                Data = "Bearer 123asd",
                Message = "Welcome Username",
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}