using Cart.Application.DTO;
using Common.Application.DTO.ValueObjects;
using Common.Domain.ValueObjects;
using Common.Infrastructure.Contracts;
using Mapster;
using MapsterMapper;
using Product.Infrastructure.gRPC.Protos;

namespace Cart.Application.Pipelines.AssociatePriceAndName.Cart
{
    public class SearchQuerieSinglePipeline : ISearchQuerieSingleBehavior<Domain.Entities.Cart, CartDTO>
    {
        private readonly ProductService.ProductServiceClient _productClient;
        private readonly IMapper _mapper;

        public int Order => 1;
        public SearchQuerieSinglePipeline(ProductService.ProductServiceClient productClient, IMapper mapper)
        {
            _productClient = productClient;
            _mapper = mapper;
        }

        public async Task<IResponse> OnAfterAsync(IResponse response, CancellationToken cancellationToken)
        {
            if(response is IResponse<CartDTO> res)
            {
                var request = new GetProductsRequest();
                request.Ids.AddRange(res.Data.Items.Select(x => x.ProductId.ToString()));
                var productsInfo = await _productClient.GetProductsInfoAsync(request);
                foreach (var p in productsInfo.Products)
                {
                    var item = res.Data.Items.FirstOrDefault(x => x.ProductId.Equals(Guid.Parse(p.Id)));
                    if(item != null)
                    {
                        item.Name = p.Name;
                        if(p.Price != null)
                        item.Price = p.Price.Adapt<PriceDTO>(_mapper.Config);
                    }
                }
                return res;
            }
            return response;
        }

        public async Task<IQuerieFilter> OnBeforeAsync(IQuerieFilter filter, CancellationToken cancellationToken)
        => filter;
    }
}
