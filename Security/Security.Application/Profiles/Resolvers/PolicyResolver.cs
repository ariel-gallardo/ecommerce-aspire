using AutoMapper;
using Common.Infrastructure.Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Security.Domain.Entities;
using Security.Infrastructure.Messaging.Messages.Request;

namespace Security.Application.Profiles.Resolvers
{
    public class PolicyResolver : IValueResolver<CreatePermissionRequest, Permission, Policy>
    {
        private readonly IWebHostEnvironment _env;

        public PolicyResolver(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Policy Resolve(CreatePermissionRequest source, Permission destination, Policy destMember, ResolutionContext context)
        {
            if (_env.EnvironmentName == "Testing")
                return Policy.Public;
            return Policy.Unknown;
        }
    }

}
