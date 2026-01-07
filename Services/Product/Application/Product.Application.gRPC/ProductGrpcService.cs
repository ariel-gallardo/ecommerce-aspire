using Common.Infrastructure.Contracts;
using Grpc.Core;
using Mapster;
using MapsterMapper;
using Product.Domain.Filters.Querie;
using Product.Infrastructure.gRPC.Protos;

namespace Product.Application.gRPC
{
    public class ProductGrpcService : ProductService.ProductServiceBase, IGrpcServiceServer
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductGrpcService(IUnitOfWork unitOfwork, IMapper mapper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
        }

        public override async Task<GetProductResponse> GetProductInfo(GetProductRequest request, ServerCallContext context)
        {
            var filters = request.Adapt<ProductQuerieFilter>(_mapper.Config);
            var product = await _unitOfWork.SearchFirstAsync<Domain.Entities.Product,ProductInfo>(filters, context.CancellationToken);
            var response = new GetProductResponse { Product = product };
            return response;
        }

        public override async Task<GetProductsResponse> GetProductsInfo(GetProductsRequest request, ServerCallContext context)
        {
            var filters = request.Adapt<ProductQuerieFilter>(_mapper.Config);
            filters.TakeAll = true;
            var products = await _unitOfWork.SearchAsync<Domain.Entities.Product, ProductInfo>(filters, context.CancellationToken);
            var response = new GetProductsResponse();
            response.Products.AddRange(products.Items);
            return response;
        }
    }
}
