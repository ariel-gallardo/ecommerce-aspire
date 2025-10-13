using Common.Api.CustomAttributes;

namespace Common.Application.DTOS.Entities.User
{
    [IgnoreAuditable]
    [IgnoreIdentifiable]
    public class UserRegisterDTO : UserDTO
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
