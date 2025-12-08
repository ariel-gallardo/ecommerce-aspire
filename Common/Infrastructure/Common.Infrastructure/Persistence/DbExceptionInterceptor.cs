using Logs.Infrastructure.Messaging.Messages.Request;
using MassTransit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;

namespace Common.Infrastructure.Persistence
{
    public class DbExceptionInterceptor : DbCommandInterceptor
    {

        public DbExceptionInterceptor()
        {
        }

        public override void CommandFailed(
            DbCommand command,
            CommandErrorEventData eventData)
        {

            base.CommandFailed(command, eventData);
            throw eventData.Exception;
        }
    }
}
