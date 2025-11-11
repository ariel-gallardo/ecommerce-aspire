using Common.Api.CustomAttributes;
using Common.Contracts.DTO.ABM;

namespace Security.Application.DTO
{
    [IgnoreIdentifiable]
    public class UserRegisterDTO : UserDTO, IAddDTO, IUpdateDTO
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
