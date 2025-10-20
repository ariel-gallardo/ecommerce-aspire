using Common.Application.Contracts.Services;
using AutoMapper;
using Common.Domain.Contracts.Repositories;
using Common.Domain.Contracts.Services;
using Common.Domain.Filters.Queries;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Common.Domain.Entities;
using Common.Infrastructure.Entities;
using Common.Application.DTO.Entities.User;
using Common.Application.DTO.Entities.Base;

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
            var filters = _mapper.Map<UserQuerieFilter>(dto);
            var user = await _unitOfWork.SearchOneAsync<User>(filters, cancellationToken);
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

        public async Task<Response> RegisterUser(UserRegisterDTO dto, CancellationToken cancellationToken)
        {
            var filters = _mapper.Map<UserQuerieFilter>(dto);
            if(!await _unitOfWork.ExistsAsync<User>(filters, cancellationToken))
            {
                return new Response<UserDTO>
                {
                    Data = _mapper.Map<UserDTO>(await _unitOfWork.AddAsync<UserRegisterDTO, User, UserDTO>(dto, cancellationToken)),
                    Message = "Account created successfully.",
                    StatusCode = StatusCodes.Status201Created
                };
            }else
            {
                return new Response
                {
                    Message = "User already exists.",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
