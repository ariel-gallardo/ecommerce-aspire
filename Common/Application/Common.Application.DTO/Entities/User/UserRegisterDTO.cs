using Common.Api.CustomAttributes;
using Common.Application.DTO.Entities.Base;
using Common.Contracts.DTO.ABM;

namespace Common.Application.DTO.Entities.User
{
    [IgnoreIdentifiable]
    public class UserRegisterDTO : UserDTO, IAddDTO, IUpdateDTO
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
