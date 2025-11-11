
namespace Common.Domain.ValueObjects
{
    public class Coordinates
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Coordinates coordinates &&
                   Latitude == coordinates.Latitude &&
                   Longitude == coordinates.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }
    }
}
