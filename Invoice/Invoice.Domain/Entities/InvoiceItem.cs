
using Common.Domain.Entities.Base;
using Common.Domain.ValueObjects;

namespace Invoice.Domain.Entities
{
    public class InvoiceItem : AuditableEntity
    {
        public Guid ProductId { get; set; }
        public Price Price { get; set; }
        public Quantity Quantity { get; set; }
        public virtual Invoice Invoice {get;set;}
        public Guid InvoiceId { get; set; }

    }
}
