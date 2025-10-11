using Common.Domain.DTOS.Base.Entities;

namespace Application.DTOS.Entities
{
    public class CoordinatesDTO : DTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CoordinatesDTO dTO &&
                   base.Equals(obj) &&
                   Latitude == dTO.Latitude &&
                   Longitude == dTO.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Latitude, Longitude);
        }
    }
}
