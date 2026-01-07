
using Common.Domain.Entities;
using Mapster;
using Security.Application.DTO;
using Security.Domain.Filters.Queries;

namespace Security.Application.Profiles
{
    
	public class UserProfile : IRegister 
    {
        public void Register(TypeAdapterConfig config)        {
            config.NewConfig<User, UserDTO>().TwoWays();
            config.NewConfig<UserLoginDTO, UserQuerieFilter>().TwoWays();
            config.NewConfig<UserRegisterDTO, UserQuerieFilter>().TwoWays();
            config.NewConfig<UserRegisterDTO, User>().TwoWays();

            #region Claims
            config.NewConfig<User, UserClaimsDTO>()
                .ConstructUsing(src => new UserClaimsDTO(src));
            #endregion
        }
    }
}
