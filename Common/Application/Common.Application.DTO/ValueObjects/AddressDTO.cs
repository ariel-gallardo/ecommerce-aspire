namespace Common.Application.DTO.ValueObjects
{
    public class AddressDTO : Base.Entities.DTO
    {
        public string Street { get; set; }
        public int Number { get; set; }
        public string Neighborhood { get; set; }
        public string Description { get; set; }
        public CoordinatesDTO Coordinates { get; set; }
    }
}
