using Common.Extensions;
using Mapster;
using Product.Infrastructure.gRPC.Protos;
using Product.Domain.Filters.Querie;
using Common.Application.DTO.ValueObjects;
namespace Product.Application.gRPC.Profiles
{
    public class ProductProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Common.Domain.ValueObjects.Price,Price>()
                .Map(dest => dest.Value, src => src.Value)
                .Map(dest => dest.Unit, src => src.Unit.AsStringUsingMemberValue());
            config.NewConfig<Price, PriceDTO>();
            config.NewConfig<Domain.Entities.Product, ProductInfo>();
            config.NewConfig<GetProductRequest, ProductQuerieFilter>();
        }
    }
}
