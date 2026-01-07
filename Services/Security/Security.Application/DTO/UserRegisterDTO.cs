using Common.Api.CustomAttributes;
using Common.Infrastructure.Contracts;

namespace Security.Application.DTO
{
    [IgnoreIdentifiable]
    public class UserRegisterDTO : UserDTO, IAddDTO, IUpdateDTO
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
