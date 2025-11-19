using AutoMapper;
using Common.Contracts;
using Common.Infrastructure.Entities.Const;
using Common.Infrastructure.Repositories;
using Moq;
using Security.Infrastructure.Contracts;
using System.Reflection;

namespace Common.Unit
{
    public abstract class UnitTest
    {
        private readonly IAuthServices _authServices;
        private readonly Assembly[] _autoMapperAssemblies;
        private readonly IMapper _mapper;
        protected CancellationToken CancellationToken = default;

        public UnitTest(params Assembly[] autoMapperAssemblies)
        {
            var authMock = new Mock<IAuthServices>();
            authMock.Setup(x => x.Id).Returns(SecurityConst.InternalAdminId);
            authMock.Setup(x => x.IsAuthenticated).Returns(true);
            _authServices = authMock.Object;
            _autoMapperAssemblies = autoMapperAssemblies;
            if(_autoMapperAssemblies.Length > 0)
            _mapper = new MapperConfiguration(x => x.AddMaps(_autoMapperAssemblies)).CreateMapper();
        }

    }
}
