using Common.Application.DTO.Base.Entities;
using Common.Contracts.DTO.ABM;

namespace Client.Application.DTO
{
    public class ClientDTO : AuditableDTO, IAddDTO, IUpdateDTO, IResultDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public string FechaNacimiento { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
    }
}
