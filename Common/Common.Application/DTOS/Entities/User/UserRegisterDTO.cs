namespace Application.DTOS.Entities.User
{
    public class UserRegisterDTO : UserDTO
    {
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
