namespace Common.Application.DTO.Entities.Base
{
    public class CoordinatesDTO : DTO.Base.Entities.DTO
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
