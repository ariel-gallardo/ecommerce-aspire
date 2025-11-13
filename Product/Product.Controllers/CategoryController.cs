using Common.Api.Controllers;
using Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTO;
using Product.Controllers.Contracts;
using Product.Domain.Filters.Querie;

namespace Product.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : CommonController<Domain.Entities.Category, CategoryDTO, CategoryDTO, CategoryDTO, CategoryQuerieFilter>, ICategoryController
    {
        public CategoryController(ICommonServices services) : base(services)
        {
        }
    }
}
