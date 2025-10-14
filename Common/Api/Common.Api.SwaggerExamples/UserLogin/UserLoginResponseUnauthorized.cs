using Common.Domain.DTOS.Base.Entities;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Api.SwaggerExamples.UserLogin
{
    public class UserLoginResponseUnauthorized : IExamplesProvider<Response>
    {
        public Response GetExamples()
        {
            return new Response
            {
                Message = "Invalid credentials.",
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
