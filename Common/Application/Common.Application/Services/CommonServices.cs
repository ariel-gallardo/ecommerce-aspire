using Common.Contracts;
using Common.Contracts.DTO.ABM;
using Common.Contracts.DTO.Base;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using Common.Domain.Contracts.Repositories;
using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Services
{
    public class CommonServices : ICommonServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommonServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Add
        public async Task<BaseResponse> AddAsync<AddDTO, DomainEntity, ResultDTO>(AddDTO entity, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var result = await _unitOfWork.AddAsync<AddDTO,DomainEntity,ResultDTO>(entity, cancellationToken);
            return new Response<ResultDTO>
            {
                Data = result,
                Message = $"{typeof(DomainEntity).Name} created.",
                StatusCode = StatusCodes.Status201Created
            };
        }

        public async Task<BaseResponse> AddAsync<AddDTO, DomainEntity, ResultDTO>(IList<AddDTO> entities, CancellationToken cancellationToken)
            where AddDTO : class, IEntityDTO, IAddDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var results = await _unitOfWork.AddAsync<AddDTO, DomainEntity, ResultDTO>(entities, cancellationToken);
            return new Response<IList<ResultDTO>>
            {
                Data = results,
                Message = $"{typeof(DomainEntity).Name} created.",
                StatusCode = StatusCodes.Status201Created
            };
        }
        #endregion

        #region Update
        public async Task<BaseResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(UpdateDTO entity, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var result = await _unitOfWork.UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(entity, cancellationToken);
            return new Response<ResultDTO>
            {
                Data = result,
                Message = $"{typeof(DomainEntity).Name} updated.",
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<BaseResponse> UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(IList<UpdateDTO> entities, CancellationToken cancellationToken)
            where UpdateDTO : class, IEntityDTO, IUpdateDTO
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            var results = await _unitOfWork.UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(entities, cancellationToken);
            return new Response<IList<ResultDTO>>
            {
                Data = results,
                Message = $"{typeof(DomainEntity).Name} updated.",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #endregion

        #region Delete
        public async Task<BaseResponse> DeleteAsync<DomainEntity>(Guid entityId, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            await _unitOfWork.DeleteAsync<DomainEntity>(entityId, cancellationToken);
            return new BaseResponse
            {
                Message = $"{typeof(DomainEntity).Name} - {entityId.ToString()} - deleted.",
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<BaseResponse> DeleteAsync<DomainEntity>(IList<Guid> entityIds, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
        {
            await _unitOfWork.DeleteAsync<DomainEntity>(entityIds, cancellationToken);
            return new BaseResponse
            {
                Message = $"{typeof(DomainEntity).Name} - {string.Join(",",entityIds)} - deleted.",
                StatusCode = StatusCodes.Status200OK
            };
        }
        #endregion

        #region Search
        public async Task<BaseResponse> SearchAsync<DomainEntity, ResultDTO>(Guid entityId, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            BaseResponse response;
            var result = await _unitOfWork.SearchAsync<DomainEntity>(entityId, cancellationToken);
            if(result != null)
            {
                response = new Response<DomainEntity> { Data = result, Message = $"{typeof(DomainEntity).Name} found.", StatusCode = StatusCodes.Status200OK };
            }
            else
            {
                response = new BaseResponse 
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"{typeof(DomainEntity).Name} not found."
                };
            }
            return response;
        }

        public async Task<BaseResponse> SearchAsync<DomainEntity, ResultDTO>(IQuerieFilter filters, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            BaseResponse response;
            var result = await _unitOfWork.SearchAsync<DomainEntity,ResultDTO>(filters, cancellationToken);
            if (result.Any())
            {
                response = new Response<IPagedList<ResultDTO>> { Data = result, Message = $"{typeof(DomainEntity).Name} found.", StatusCode = StatusCodes.Status200OK };
            }
            else
            {
                response = new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"{typeof(DomainEntity).Name} not found."
                };
            }
            return response;
        }

        public async Task<BaseResponse> SearchAsync<DomainEntity, ResultDTO>(IList<Guid> entityIds, int page, int pageSize, CancellationToken cancellationToken)
            where DomainEntity : class, IEntity
            where ResultDTO : class, IEntityDTO, IResultDTO
        {
            BaseResponse response;
            var result = await _unitOfWork.SearchAsync<DomainEntity>(entityIds, page, pageSize, cancellationToken);
            if (result.Any())
            {
                response = new Response<IList<DomainEntity>> { Data = result, Message = $"{typeof(DomainEntity).Name} found.", StatusCode = StatusCodes.Status200OK };
            }
            else
            {
                response = new BaseResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"{typeof(DomainEntity).Name} not found."
                };
            }
            return response;
        }
        #endregion

    }
}
