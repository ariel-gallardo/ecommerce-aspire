using Common.Infrastructure.Messages.Contracts;
using MassTransit;
using Security.Infrastructure.Messaging.Messages.Development;

namespace Security.Infrastructure.Messaging.Consumer.Development.Contracts
{
    public interface IUserAdminCreatedConsumer : IDevelopmentCosumer, IConsumer<UserAdminCreated>
    {
    }
}
