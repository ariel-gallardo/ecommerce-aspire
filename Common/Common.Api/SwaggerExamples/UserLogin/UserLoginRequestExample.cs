using Common.Application.DTOS.Entities.User;
using Swashbuckle.AspNetCore.Filters;

namespace Common.Api.SwaggerExamples.UserLogin
{
    public class UserLoginRequestExample : IMultipleExamplesProvider<UserLoginDTO>
    {
        public IEnumerable<SwaggerExample<UserLoginDTO>> GetExamples()
        {
            yield return SwaggerExample.Create("Email+Password 1",new UserLoginDTO
            {
                Email = "user_email_1@mail.com",
                Password = "123456aA$",
            });

            yield return SwaggerExample.Create("Username+Password 1", new UserLoginDTO
            {
                Password = "123456aA$",
                Username = "user_name_1"
            });

            yield return SwaggerExample.Create("Email+Password 2", new UserLoginDTO
            {
                Email = "user_email_2@mail.com",
                Password = "123456aA$",
            });

            yield return SwaggerExample.Create("Username+Password 2", new UserLoginDTO
            {
                Password = "123456aA$",
                Username = "user_name_2"
            });
        }
    }
}

