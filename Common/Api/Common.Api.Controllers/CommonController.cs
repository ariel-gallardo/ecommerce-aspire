using Common.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class CommonController<Key,DomainEntity, AddDTO, UpdateDTO, ResultDTO, QuerieFilterEntity> : ControllerBase, 
        ICommonController<Key,DomainEntity, AddDTO, UpdateDTO, ResultDTO,QuerieFilterEntity>
        
        where DomainEntity : class, IEntity
        where AddDTO : class, IEntityDTO, IAddDTO
        where UpdateDTO : class, IEntityDTO, IUpdateDTO
        where ResultDTO : class, IEntityDTO, IReadDTO
        where QuerieFilterEntity : class, IQuerieFilter
    {
        protected readonly ICommonServices _services;
        protected CommonController(ICommonServices services)
        {
            _services = services;
        }

        #region Add
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddDTO entity, CancellationToken cancellationToken)
        {
            var response = await _services.AddAsync<AddDTO,DomainEntity,ResultDTO>(entity, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("range")]
        public async Task<IActionResult> AddAsync([FromBody] IList<AddDTO> entities, CancellationToken cancellationToken)
        {
            var response = await _services.AddAsync<AddDTO, DomainEntity, ResultDTO>(entities, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDTO entity, CancellationToken cancellationToken)
        {
            var response = await _services.UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(entity, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("range")]
        public async Task<IActionResult> UpdateAsync([FromBody] IList<UpdateDTO> entities, CancellationToken cancellationToken)
        {
            var response = await _services.UpdateAsync<UpdateDTO, DomainEntity, ResultDTO>(entities, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Delete
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] Key entityId, CancellationToken cancellationToken)
        {
            var response = await _services.DeleteAsync<Key,DomainEntity>(entityId, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("range")]
        public async Task<IActionResult> DeleteAsync([FromBody] IList<Key> entityIds, CancellationToken cancellationToken)
        {
            var response = await _services.DeleteAsync<Key,DomainEntity>(entityIds, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        #endregion

        #region Search

        [HttpGet]
        public async Task<IActionResult> SearchAsync([FromQuery] Key entityId, CancellationToken cancellationToken)
        {
            var response = await _services.SearchAsync<Key,DomainEntity,ResultDTO>(entityId, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("filters")]
        public async Task<IActionResult> SearchAsync([FromQuery] QuerieFilterEntity filters, CancellationToken cancellationToken)
        {
            var response = await _services.SearchAsync<DomainEntity,ResultDTO>(filters, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("filters-first")]
        public async Task<IActionResult> SearchFirstAsync([FromQuery] QuerieFilterEntity filters, CancellationToken cancellationToken)
        {
            var response = await _services.SearchFirstAsync<DomainEntity, ResultDTO>(filters, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("ids")]
        public async Task<IActionResult> SearchAsync([FromQuery] IList<Key>? entityIds, [FromQuery] int page, [FromQuery] int pageSize, CancellationToken cancellationToken)
        {
            if (entityIds == null) entityIds = Array.Empty<Key>();
            var response = await _services.SearchAsync<Key,DomainEntity, ResultDTO>(entityIds,page, pageSize, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        #endregion
    }
}
