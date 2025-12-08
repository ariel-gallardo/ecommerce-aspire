using Common.Infrastructure.Entities.Enums;
using FluentValidation;
using Security.Application.DTO;
using System.Runtime.Serialization;

namespace Security.Application.Rules
{
    public class PermissionDTOValidator : AbstractValidator<PermissionDTO>
    {
        public PermissionDTOValidator()
        {

            RuleFor(x => x.Policy)
                .NotEmpty().WithMessage("Policy es obligatorio.")
                .Must(BeValidPolicy).WithMessage("Policy no es válido.");


            When(x => !string.IsNullOrWhiteSpace(x.Url), () =>
            {
                RuleFor(x => x.Controller)
                    .Null().WithMessage("No debe enviar Controller si indica Url.");

                RuleFor(x => x.Action)
                    .Null().WithMessage("No debe enviar Action si indica Url.");
            });


            When(x => string.IsNullOrWhiteSpace(x.Url), () =>
            {
                RuleFor(x => x.Controller)
                    .NotNull().NotEmpty().WithMessage("Controller es obligatorio si no se envía Url.");

                RuleFor(x => x.Action)
                    .NotNull().NotEmpty().WithMessage("Action es obligatorio si no se envía Url.");
            });
        }

        private bool BeValidPolicy(string policy)
        {
            if (string.IsNullOrWhiteSpace(policy))
                return false;

            return Enum.GetNames(typeof(Policy))
                       .Select(p => p)
                       .Any(p => EnumMemberValue(typeof(Policy), p) == policy);
        }

        private string EnumMemberValue(Type enumType, string name)
        {
            var member = enumType.GetMember(name).First();
            var attr = member
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .SingleOrDefault();

            return attr?.Value ?? name;
        }
    }
}
