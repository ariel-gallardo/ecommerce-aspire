using Common.Domain.Entities.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security.Domain.Entities;
using Security.Infrastructure.Entities;
using System.Linq.Expressions;
using System.Linq;

namespace Security.Domain.Filters.Queries
{
    public class UserQuerieFilter : QuerieFilter
    {
        [FromQuery]
        public string Username { get; set; }
        [FromQuery]
        public string Email { get; set; }
        [FromQuery]
        public Role? Role { get; set; }
        [FromQuery]
        public Guid? PersonaId { get; set; }

        #region Expressions
        private Expression<Func<User, bool>>? FindByUserName
        {
            get => !string.IsNullOrEmpty(Username) ? x => x.Username == Username : null;
        }

        private Expression<Func<User, bool>>? FindByEmail
        {
            get => !string.IsNullOrEmpty(Email) ? x => x.Email == Email : null;
        }
        private Expression<Func<User, bool>>? FindByPersonaId
        {
            get => PersonaId.HasValue ? x => PersonaId == x.PersonaId : null;
        }
        private Expression<Func<User,bool>>? FindByRole
        {
            get => Role.HasValue ? x => x.Rol == Role : null;
        }
        #endregion
    }
}
