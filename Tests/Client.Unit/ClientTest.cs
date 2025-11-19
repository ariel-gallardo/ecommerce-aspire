using Client.Application.Profiles;
using Common.Unit;
using System.Reflection;


namespace Client.Unit
{
    public abstract class ClientTest : UnitTest
    {
        public ClientTest() : base(typeof(ClientProfile).Assembly)
        {
        }
    }
}
