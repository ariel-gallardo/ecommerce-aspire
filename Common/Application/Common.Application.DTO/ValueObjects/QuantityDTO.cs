namespace Common.Application.DTO.ValueObjects
{
    public class QuantityDTO : Base.Entities.DTO
    {
        public decimal Value { get; set; }
        public string Unit { get; set; }
    }
}
