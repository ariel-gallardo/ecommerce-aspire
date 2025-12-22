using Common.Api.Controllers;
using Common.Infrastructure.Contracts;
using Product.Application.DTO;
using Product.Controllers.Contracts;
using Product.Domain.Filters.Querie;

namespace Product.Controllers
{
    public class CategoryController : CommonController<Guid, Domain.Entities.Category, CategoryDTO, CategoryDTO, CategoryDTO, CategoryQuerieFilter>, ICategoryController
    {
        public CategoryController(ICommonServices services) : base(services)
        {
        }
    }
}
