using Common.Application.DTOS.Entities.User;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Api.SwaggerExamples.UserRegister
{
    public class UserRegisterRequestExample : IMultipleExamplesProvider<UserRegisterDTO>
    {
        public IEnumerable<SwaggerExample<UserRegisterDTO>> GetExamples()
        {
            yield return SwaggerExample.Create("User Register", new UserRegisterDTO
            {
                Email = "user_email_register@mail.com",
                Username = "user_register",
                Password = "123456aA$",
            });
        }
    }
}
