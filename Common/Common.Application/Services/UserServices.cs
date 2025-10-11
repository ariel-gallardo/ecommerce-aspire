using Application.Contracts.Services;
using Application.DTOS.Entities.User;
using AutoMapper;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Domain.DTOS.Base.Entities;
using Common.Domain.Filters.Queries;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Common.Application
{
    public class UserServices : IUserServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthServices _authServices;

        public UserServices(IMapper mapper, IUnitOfWork unitOfWork, IAuthServices authServices)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _authServices = authServices;
        }
        public async Task<Response> AuthUser(UserLoginDTO dto, CancellationToken cancellationToken)
        {
            var filters = _mapper.Map<UserQuerieFilters>(dto);
            var user = await _unitOfWork.FirstOrDefaultByQuerieFiltersAsync(filters, cancellationToken);
            if (user != null && _authServices.VerifyPassword(dto.Password, user.Password))
            {
                var userClaims = _mapper.Map<Claim[]>(user);
                return new Response<string>
                {
                    Data = _authServices.GenerateToken(userClaims),
                    Message = $"Welcome {user.Username}",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            return new Response
            {
                Message = "Invalid credentials.",
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
