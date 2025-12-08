using Common.Contracts;
using Product.Application.DTO;
using Product.Domain.Entities;
using Product.Domain.Filters.Querie;

namespace Product.Controllers.Contracts
{
    public interface ICategoryController : ICommonController<Guid, Category, CategoryDTO, CategoryDTO, CategoryDTO, CategoryQuerieFilter>
    {
    }
}
