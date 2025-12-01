using Common.Api.Controllers;
using Common.Contracts;
using Common.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Security.Application.DTO;
using Security.Controllers.Contracts;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Controllers
{
    public class PermissionController : CommonController<Permission, PermissionDTO, PermissionDTO, PermissionDTO, PermissionQuerieFilter>, IPermissionController
    {
        public PermissionController(ICommonServices services) : base(services)
        {
        }

        [HttpHead]
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
