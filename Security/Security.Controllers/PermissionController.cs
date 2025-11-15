using Common.Api.Controllers;
using Common.Contracts;
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
    }
}
