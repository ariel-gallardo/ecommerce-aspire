namespace Common.Application.DTO.Entities.Base
{
    public class AddressDTO : DTO.Base.Entities.DTO
    {
        public string Street { get; set; }
        public int Number { get; set; }
        public string Neighborhood { get; set; }
        public string Description { get; set; }
    }
}
