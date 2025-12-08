using Common.Contracts;
using Product.Application.DTO;
using Product.Domain.Filters.Querie;

namespace Product.Controllers.Contracts
{
    public interface IProductController : ICommonController<Guid, Domain.Entities.Product, ProductDTO, ProductDTO, ProductDTO, ProductQuerieFilter>
    {
    }
}
