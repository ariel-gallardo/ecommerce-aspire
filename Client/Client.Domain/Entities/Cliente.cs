using Common.Domain.Entities.Base;

namespace Client.Domain.Entities
{
    public class Cliente : AuditableEntity
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
    }
}
