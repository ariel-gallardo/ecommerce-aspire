namespace Common.Domain.Exceptions
{
    public class ForbiddenException : UnauthorizedAccessException
    {
        public ForbiddenException(Exception? exception = null) : base("You must be logged in to perform this operation.", exception)
        {

        }
    }
}
