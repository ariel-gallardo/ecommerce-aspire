using Common.Api.Controllers;
using Common.Contracts;
using Product.Application.DTO;
using Product.Controllers.Contracts;
using Product.Domain.Filters.Querie;

namespace Product.Controllers
{
    public class CategoryController : CommonController<Domain.Entities.Category, CategoryDTO, CategoryDTO, CategoryDTO, CategoryQuerieFilter>, ICategoryController
    {
        public CategoryController(ICommonServices services) : base(services)
        {
        }
    }
}
