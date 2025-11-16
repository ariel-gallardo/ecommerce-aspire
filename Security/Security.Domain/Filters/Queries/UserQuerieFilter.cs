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
        public RoleEnum? Role { get; set; }
        [FromQuery]
        public Guid? PersonaId { get; set; }

        #region Expressions
        private Expression<Func<User, bool>> FindByUserName
        {
            get => x => !string.IsNullOrEmpty(Username) && x.Username == Username;
        }

        private Expression<Func<User, bool>> FindByEmail
        {
            get => x => !string.IsNullOrEmpty(Email) && x.Email == Email;
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
