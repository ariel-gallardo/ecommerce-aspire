namespace Common.Infrastructure.Entities.Const
{
    public class SecurityConst
    {
        public static Guid InternalAdminId => new Guid("00000000-0000-0000-0000-000000000001");
        public const string AuthenticationBearer = "Bearer";
        public const string AuthenticationInternal = "Internal";
    }
}
