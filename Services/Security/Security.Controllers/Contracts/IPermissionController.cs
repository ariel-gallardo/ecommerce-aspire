using Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Security.Application.DTO;
using Security.Domain.Entities;
using Security.Domain.Filters.Queries;

namespace Security.Controllers.Contracts
{
    public interface IPermissionController : ICommonController<Permission,PermissionDTO, PermissionDTO, PermissionDTO,PermissionQuerieFilter>
    {
        Task<IActionResult> CanAccess(string url, CancellationToken cancellationToken);
    }
}
