
namespace Common.Domain.ValueObjects
{
    public class Address
    {
        public string Street { get; set; }
        public int Number { get; set; }
        public string Neighborhood { get; set; }
        public string Description { get; set; }
        public virtual Coordinates Coordinates { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Address address &&
                   Street == address.Street &&
                   Number == address.Number &&
                   Neighborhood == address.Neighborhood &&
                   Description == address.Description &&
                   EqualityComparer<Coordinates>.Default.Equals(Coordinates, address.Coordinates);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street, Number, Neighborhood, Description, Coordinates);
        }
    }
}
