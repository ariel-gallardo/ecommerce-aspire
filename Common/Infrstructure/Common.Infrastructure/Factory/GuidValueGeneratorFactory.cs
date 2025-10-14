using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Common.Infrastructure.Factory
{
    public class GuidValueGeneratorFactory : ValueGeneratorFactory
    {
        public override ValueGenerator Create(IProperty property, ITypeBase typeBase)
        {
            return new GuidValueGenerator();
        }

        private class GuidValueGenerator : ValueGenerator<Guid>
        {
            public override bool GeneratesTemporaryValues => false;

            public override Guid Next(EntityEntry entry)
            {
                return Guid.NewGuid();
            }
        }
    }
}
