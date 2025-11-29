using Client.Application.DTO;
using Client.Application.Profiles;
using Client.Controllers;
using Client.Controllers.Contracts;
using Client.Domain.Filters;
using Client.Infrastructure.Persistence;
using Client.Infrastructure.Seeders;
using Common.Contracts;
using Common.Infrastructure.Entities;
using Common.UnitTest;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace UnitTests.Client
{
    public class ClientTest : UnitTest<ClientDbContext>
    {
        private readonly IClientesController _controller;

        public ClientTest() 
            : base(new Assembly[] { typeof(ClientProfile).Assembly }, 
                  new Assembly[] {}, new Assembly[] { typeof(ClientSeeder).Assembly }, new Assembly[] { },
                  new Assembly[] {typeof(ClientesController).Assembly })
        {
            _controller = _services.GetService<IClientesController>();
        }

        [Fact]
        protected override async Task Should_Create_Entity_Async()
        {
            var newEntity = new ClientDTO
            {
                Nombre = "Ariel",
                Apellido = "Gallardo",
                Cuit = "10-00000000-0",
                Email = "ariel.gallardo.dev@gmail.com",
                FechaNacimiento = "01/01/1900",
                RazonSocial = "Developer A",
                TelefonoCelular = "2616557585"
            };
            IActionResult response = await _controller.AddAsync(newEntity, default);
            Assert.NotNull(response);
            Assert.IsType<ObjectResult>(response);
            Response<ClientDTO> result = ((ObjectResult)response).Value as Response<ClientDTO>;
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.NotNull(result.Data);
            result.Data.Should().BeEquivalentTo(newEntity, options =>
                options
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.UpdatedAt)
                .Excluding(x => x.DeletedAt)
                .Excluding(x => x.Id)
            );
        }

        [Fact]
        protected override async Task Should_Delete_Entity_Async()
        {
            IActionResult response = await _controller.DeleteAsync(ulong.Parse($"{_appSettings.QuantityToGenerate - 1}"), default);
            Assert.NotNull(response);
            BaseResponse result = ((ObjectResult)response).Value as BaseResponse;
            Assert.IsType<ObjectResult>(response);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        protected override async Task Should_Get_All_Async()
        {
            var currentPage = 1;

            var filters = new ClientQuerieFilter
            {
                PageSize = 5,
            };
            var totalPages = _appSettings.QuantityToGenerate / filters.PageSize;
            var expectedEntity = Enumerable.Range(1, filters.PageSize).Select(i => new ClientDTO
            {
                Id = ulong.Parse(i.ToString()),
                Nombre = $"Nombre {i}",
                Apellido = $"Apellido {i}",
                Cuit = $"00-{i.ToString().PadLeft(8, '0')}-0",
                Email = $"email_{i}@mail.com",
                FechaNacimiento = "01/01/2000",
                RazonSocial = $"Razon Social {i}",
                TelefonoCelular = $"{i.ToString().PadLeft(10, '0')}"
            });


            ulong id = 1L;

            IActionResult response = await _controller.SearchAsync(filters, default);
            Assert.NotNull(response);
            Assert.IsType<ObjectResult>(response);
            Response<IPagedList<ClientDTO>> result = ((ObjectResult)response).Value as Response<IPagedList<ClientDTO>>;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(filters.PageSize, result.Data.Count);
            Assert.Equal(totalPages, result.Data.TotalPages);
            Assert.Equal(currentPage, result.Data.CurrentPage);

            result.Data.Should().BeEquivalentTo(expectedEntity, options =>
                options
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.UpdatedAt)
                .Excluding(x => x.DeletedAt)
            );
        }

        [Fact]
        protected override async Task Should_Get_By_Id_Async()
        {
            var expectedEntity = new ClientDTO
            {
              Id= 1,
              Nombre = "Nombre 1",
              Apellido = "Apellido 1",
              Cuit = "00-00000001-0",
              Email = "email_1@mail.com",
              FechaNacimiento = "01/01/2000",
              RazonSocial = "Razon Social 1",
              TelefonoCelular = "0000000001",
            };

            ulong id = 1L;

            IActionResult response = await _controller.SearchAsync(id, default);
            Assert.NotNull(response);
            Assert.IsType<ObjectResult>(response);
            Response<ClientDTO> result = ((ObjectResult)response).Value as Response<ClientDTO>;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);

            result.Data.Should().BeEquivalentTo(expectedEntity, options =>
                options
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.UpdatedAt)
                .Excluding(x => x.DeletedAt)
            );
        }

        [Fact]
        protected override async Task Should_Search_Async()
        {
            var currentPage = 1;
            var totalPages = 1;
            var totalItems = 1;
            var expectedEntity = new List<ClientDTO>{ new ClientDTO
            {
                Id = ulong.Parse(10.ToString()),
                Nombre = $"Nombre {10}",
                Apellido = $"Apellido {10}",
                Cuit = $"00-{10.ToString().PadLeft(8, '0')}-0",
                Email = $"email_{10}@mail.com",
                FechaNacimiento = "01/01/2000",
                RazonSocial = $"Razon Social {10}",
                TelefonoCelular = $"{10.ToString().PadLeft(10, '0')}"
            } };

            var filters = new ClientQuerieFilter
            {
                Nombre = "Nombre 10",
                FechaNacimientoMin = "12/10/1999",
                FechaNacimientoMax = "01/10/2002",
                PageSize = 5
            };
            IActionResult response = await _controller.SearchAsync(filters, default);
            Assert.NotNull(response);
            Assert.IsType<ObjectResult>(response);
            Response<IPagedList<ClientDTO>> result = ((ObjectResult)response).Value as Response<IPagedList<ClientDTO>>;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(totalItems, result.Data.Count);
            Assert.Equal(totalPages, result.Data.TotalPages);
            Assert.Equal(currentPage, result.Data.CurrentPage);

            result.Data.Should().BeEquivalentTo(expectedEntity, options =>
                options
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.UpdatedAt)
                .Excluding(x => x.DeletedAt)
            );
        }
        [Fact]
        protected override async Task Should_Update_Entity_Async()
        {
            var currentPage = 1;
            var totalPages = 1;
            var totalItems = 1;
            
            var expectedEntity = new ClientUpdateDTO
            {
                Id = ulong.Parse(10.ToString()),
                Nombre = $"Nombre {10}",
                Apellido = $"Apellido {10}",
                Cuit = $"00-{10.ToString().PadLeft(8, '0')}-0",
                Email = $"email_{10}@mail.com",
                FechaNacimiento = "01/01/2000",
                RazonSocial = $"Razon Social {10}",
                TelefonoCelular = $"{10.ToString().PadLeft(10, '0')}"
            };

            expectedEntity.Nombre = "Nombre UpdatedName";

            var response = await _controller.UpdateAsync(expectedEntity, default);

            Assert.NotNull(response);
            Assert.IsType<ObjectResult>(response);
            Response<ClientDTO> result = ((ObjectResult)response).Value as Response<ClientDTO>;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Data);

            result.Data.Should().BeEquivalentTo(expectedEntity, options =>
                options
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.UpdatedAt)
                .Excluding(x => x.DeletedAt)
            );
        }
    }
}
