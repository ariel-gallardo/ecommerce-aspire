using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Invoice.Application.DTO
{
    public class InvoiceDTO : AuditableDTO, IAddDTO, IUpdateDTO
    {
        public Guid OrderId { get; set; }
        public List<InvoiceItemDTO> Items { get; set; } = new();

        public override bool Equals(object? obj)
        {
            return obj is InvoiceDTO dTO &&
                   base.Equals(obj) &&
                   OrderId.Equals(dTO.OrderId) &&
                   EqualityComparer<List<InvoiceItemDTO>>.Default.Equals(Items, dTO.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), OrderId, Items);
        }
    }
}
