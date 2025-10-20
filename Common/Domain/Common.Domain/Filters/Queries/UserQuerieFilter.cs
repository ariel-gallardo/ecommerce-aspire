using Common.Domain.Entities;
using Common.Domain.Entities.Base;
using Common.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Common.Domain.Filters.Queries
{
    public class UserQuerieFilter : QuerieFilter
    {
        [FromQuery]
        public string Username { get; set; }
        [FromQuery]
        public string Email { get; set; }
        [FromQuery]
        public RoleEnum? Role { get; set; }
        [FromQuery]
        public Guid? PersonaId { get; set; }

        #region Expressions
        private Expression<Func<User, bool>> FindByUserName
        {
            get => x => !string.IsNullOrEmpty(Username) && string.Equals(x.Username,Username,StringComparison.InvariantCultureIgnoreCase);
        }
        private Expression<Func<User, bool>> FindByEmail
        {
            get => x => !string.IsNullOrEmpty(Email) && string.Equals(x.Email, Email, StringComparison.InvariantCultureIgnoreCase);
        }
        private Expression<Func<User, bool>> FindByPersonaId
        {
            get => x => PersonaId.HasValue && PersonaId == x.PersonaId;
        }
        private Expression<Func<User,bool>> FindByRole
        {
            get => x => Role.HasValue && x.Rol == Role;
        }
        #endregion
    }
}
