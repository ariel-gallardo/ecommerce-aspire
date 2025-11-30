using AutoMapper;
using Inventory.Application.DTO;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class InventoryItemProfile : Profile
    {
        public InventoryItemProfile()
        {
            CreateMap<InventoryItemDTO, InventoryItem>().ReverseMap();
        }
    }
}
