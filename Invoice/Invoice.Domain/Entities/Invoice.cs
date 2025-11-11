using Common.Domain.Entities.Base;

namespace Invoice.Domain.Entities
{
    public class Invoice : AuditableEntity
    {
        public Guid OrderId { get; set; }
        public virtual IList<InvoiceItem> Items { get; set; }
    }
}
