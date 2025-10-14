using Common.Domain.Entities;
using Common.Domain.Entities.Base;
using System;
using System.Linq.Expressions;

namespace Common.Domain.Filters.Queries
{
    public class UserQuerieFilters : QuerieFilters<User>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid? PersonaId { get; set; }

        #region Expressions
        public Expression<Func<User, bool>> FindByUserName
        {
            get => x => !string.IsNullOrEmpty(Username) && string.Equals(x.Username,Username,StringComparison.InvariantCultureIgnoreCase);
        }
        public Expression<Func<User, bool>> FindByEmail
        {
            get => x => !string.IsNullOrEmpty(Email) && string.Equals(x.Email, Email, StringComparison.InvariantCultureIgnoreCase);
        }
        public Expression<Func<User, bool>> FindByPersonaId
        {
            get => x => PersonaId.HasValue && PersonaId == x.PersonaId;
        }
        #endregion
    }
}
