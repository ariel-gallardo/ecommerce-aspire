using Common.Extensions;
using Common.Infrastructure.Contracts;
using Grpc.Core;
using MapsterMapper;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;
using Security.Infrastructure.Contracts;
using Security.Infrastructure.gRPC.Protos;

namespace Security.Application.gRPC
{
    public class PermissionGrpcService : PermissionService.PermissionServiceBase, IGrpcServiceServer
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthServices _authServices;

        public PermissionGrpcService(IUnitOfWork unitOfWork, IMapper mapper, IAuthServices authServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authServices = authServices;
        }

        public override async Task<PermissionResponse> GetPolicy(PermissionRequest request, ServerCallContext context)
        {
            var result = new PermissionResponse();
            if (await _unitOfWork.ExistsAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(request), default))
            {
                var data = await _unitOfWork.SearchOneAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(request), default);
                result.Policy = data!.Policy.AsStringUsingMemberValue();
            }   
            return result;
        }

        public override async Task<PermissionResponse> CreatePolicy(PermissionRequest request, ServerCallContext context)
        {
            var result = new PermissionResponse();
            var filters = _mapper.Map<PermissionQuerieFilter>(request);
            if (!await _unitOfWork.ExistsAsync<Permission>(_mapper.Map<PermissionQuerieFilter>(filters), default))
            {
                await _authServices.AuthAsAdmin();
                var permission = _mapper.Map<Permission>(request);
                result.Policy = permission.Policy.AsStringUsingMemberValue();
            }
            return result;
        }
    }
}
