using Common.Api.Controllers;
using Common.Infrastructure.Contracts;
using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Security.Application.DTO;
using Security.Controllers.Contracts;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Controllers
{
    public class PermissionController : CommonController<Guid, Permission, PermissionDTO, PermissionDTO, PermissionDTO, PermissionQuerieFilter>, IPermissionController
    {
        public PermissionController(ICommonServices services) : base(services)
        {
        }

        [HttpHead("can-access")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponse))]
        public async Task<IActionResult> CanAccess([FromHeader(Name = "X-Url")] string url, CancellationToken cancellationToken)
        {
            return StatusCode(StatusCodes.Status200OK, new BaseResponse
            {
                Message = "Can Access",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
