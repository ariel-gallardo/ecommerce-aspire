using Client.Application.DTO;
using Client.Domain.Entities;
using Common.Application.Services;
using Common.Extensions;
using Moq;

namespace Client.Unit
{
    public class AddClientTest : ClientTest
    {
        private readonly ClientDTO _dto;
        private readonly Cliente _entity;
        private readonly CommonServices _clientServices;

        public AddClientTest()
        {
            _dto = new ClientDTO
            {
                Nombre = "Nombre 1",
                Apellido = "Apellido 1",
                Cuit = "11-11111111-1",
                FechaNacimiento = "11/11/2011",
                Email = "email@mail.com",
                RazonSocial = "Razon Social 1",
                TelefonoCelular = "2152325452"
            };
            _entity = new Cliente
            {
                Nombre = "Nombre 1",
                Apellido = "Apellido 1",
                Cuit = "11-11111111-1",
                FechaNacimiento = "11/11/2011".ToDate(),
                Email = "email@mail.com",
                RazonSocial = "Razon Social 1",
                TelefonoCelular = "2152325452"
            };
            _uowMock.Setup(x => x.AddAsync(_entity, CancellationToken)).ReturnsAsync(_entity);

            _clientServices = new CommonServices(_uowMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_User_And_Return_DTO()
        {
            var client = await _clientServices.AddAsync(_dto, CancellationToken);
        }
    }
}
