
using Product.Application.DTO;
using Common.Api.Controllers;
using Product.Domain.Filters.Querie;
using Product.Controllers.Contracts;
using Common.Infrastructure.Contracts;

namespace Product.Controllers
{
    public class ProductController : CommonController<Guid, Domain.Entities.Product, ProductDTO, ProductDTO, ProductDTO, ProductQuerieFilter>, IProductController
    {
        public ProductController(ICommonServices services) : base(services)
        {
        }
    }
}
