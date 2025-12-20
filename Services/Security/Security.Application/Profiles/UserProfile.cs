
using Common.Domain.Entities;
using Mapster;
using Security.Application.DTO;
using Security.Domain.Const;
using Security.Domain.Filters.Queries;
using System.Security.Claims;

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
            TypeAdapterConfig<User, UserClaimsDTO>
                .NewConfig()
                .MapToConstructor(true);
            #endregion
        }
    }
}
