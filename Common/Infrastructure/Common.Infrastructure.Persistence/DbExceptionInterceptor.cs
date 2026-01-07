using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

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
