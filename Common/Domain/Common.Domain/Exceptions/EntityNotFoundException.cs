using Common.Domain.Enums;

namespace Common.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string entityName, ActionEnum action, ulong id, Exception? innerException = null) : base($"Entity Not Found - {entityName} - Id {id.ToString()} - Action {nameof(action)}", innerException)
        {
            
        }

        public EntityNotFoundException(string entityName, ActionEnum action, IList<ulong> ids, Exception? innerException = null) : base($"Entity Not Found - {entityName} - Id {string.Join(",",ids.Select(x => x.ToString()))} - Action {nameof(action)}", innerException)
        {

        }
    }
}
