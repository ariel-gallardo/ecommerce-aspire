using Common.Domain.Enums;

namespace Common.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string entityName, ActionEnum action, Guid id, Exception? innerException = null) : base($"Entity Not Found - {entityName} - Id {id.ToString()} - Action {nameof(action)}", innerException)
        {
            
        }
    }
}
