using Client.Domain.Entities;
using Common.Domain.Entities.Base;
using Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
namespace Client.Domain.Filters
{
    public class ClientQuerieFilter : QuerieFilter
    {
        [FromQuery]
        public string Nombre { get; set; }
        [FromQuery]
        public string Apellido { get; set; }
        [FromQuery]
        public string RazonSocial { get; set; }
        [FromQuery]
        public string Cuit { get; set; }
        [FromQuery]
        public string FechaNacimientoMin { get; set; }
        [FromQuery]
        public string FechaNacimientoMax { get; set; }
        [FromQuery]
        public string TelefonoCelular { get; set; }
        [FromQuery]
        public string Email { get; set; }

        #region Expressions
        private Expression<Func<Cliente, bool>>? BuscarPorNombre
        {
            get => !string.IsNullOrWhiteSpace(Nombre) ? x => x.Nombre.Contains(Nombre) : null;
        }
        private Expression<Func<Cliente, bool>>? BuscarPorApellido
        {
            get => !string.IsNullOrWhiteSpace(Apellido) ? x => x.Apellido.Contains(Apellido) : null;
        }
        private Expression<Func<Cliente, bool>>? BuscarPorRazonSocial
        {
            get => !string.IsNullOrWhiteSpace(RazonSocial) ? x => x.RazonSocial.Contains(RazonSocial) : null;
        }

        private Expression<Func<Cliente, bool>>? BuscarPorCuit
        {
            get => !string.IsNullOrWhiteSpace(Cuit) ? x => x.Cuit.Contains(Cuit) : null;
        }

        private Expression<Func<Cliente, bool>>? BuscarPorFechaDeNacimiento
        {
            get => !string.IsNullOrWhiteSpace(FechaNacimientoMin) && !string.IsNullOrWhiteSpace(FechaNacimientoMax) ? x => x.FechaNacimiento >= FechaNacimientoMin.ToDate() && x.FechaNacimiento <= FechaNacimientoMax.ToDate() 
            : !string.IsNullOrWhiteSpace(FechaNacimientoMin) ? x => x.FechaNacimiento >= FechaNacimientoMin.ToDate()
            : !string.IsNullOrWhiteSpace(FechaNacimientoMax) ? x => x.FechaNacimiento >= FechaNacimientoMax.ToDate()
            : null;
        }
        private Expression<Func<Cliente, bool>>? BuscarPorTelefonoCelular
        {
            get => !string.IsNullOrWhiteSpace(TelefonoCelular) ? x => x.TelefonoCelular.Contains(TelefonoCelular) : null;
        }
        private Expression<Func<Cliente, bool>>? BuscarPorEmail
        {
            get => !string.IsNullOrWhiteSpace(Email) ? x => x.Email.Contains(Email) : null;
        }
        #endregion
    }
}
