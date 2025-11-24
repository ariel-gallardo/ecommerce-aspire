using Client.Application.DTO;
using FluentValidation;

namespace Client.Application.Rules
{
    public class ClientUpdateDTOValidator : AbstractValidator<ClientUpdateDTO>
    {
        public ClientUpdateDTOValidator()
        {
            RuleFor(x => x.Nombre)
            .NotNull().WithMessage("Nombre es requerido.")
            .NotEmpty().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Apellido)
                .NotNull().WithMessage("Apellido es requerido.")
                .NotEmpty().WithMessage("Apellido es requerido.");
            RuleFor(x => x.TelefonoCelular)
                .NotNull().WithMessage("Telefono es requerido.")
                .NotEmpty().WithMessage("Telefono es requerido.");
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email es requerido.")
                .NotEmpty().WithMessage("Email es requerido.")
                .EmailAddress().WithMessage("Debe tener formato de correo electronico.");
        }
    }
}
