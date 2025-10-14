namespace Common.Domain.Exceptions
{
    public class PermissionDeniedException : UnauthorizedAccessException
    {
        public PermissionDeniedException(Exception? exception = null) : base("You must be logged in to perform this operation.", exception)
        {
            
        }
    }
}
