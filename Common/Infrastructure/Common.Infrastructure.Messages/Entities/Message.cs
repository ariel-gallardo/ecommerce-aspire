using Common.Application.DTO.Base.Entities;

namespace Common.Infrastructure.Messages.Entities
{
    public abstract class Message : DTO
    {
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString();
    }
}
