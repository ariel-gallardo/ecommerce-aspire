using Client.Application.DTO;
using FluentValidation;


namespace Client.Application.Rules
{
    public class ClientDTOValidator : AbstractValidator<ClientDTO>
    {
        public ClientDTOValidator()
        {
            RuleFor(x => x.Nombre)
                .NotNull().WithMessage("Nombre es requerido.")
                .NotEmpty().WithMessage("Nombre es requerido.");
            RuleFor(x => x.Apellido)
                .NotNull().WithMessage("Apellido es requerido.")
                .NotEmpty().WithMessage("Apellido es requerido.");
            RuleFor(x => x.Cuit)
                .NotNull().WithMessage("Cuit es requerido.")
                .NotEmpty().WithMessage("Cuit es requerido.")
                .Matches(@"^\d{2}-\d{8}-\d{1}$").WithMessage("Debe tener el siguiente formato XX-XXXXXXXX-X");
            RuleFor(x => x.TelefonoCelular)
                .NotNull().WithMessage("Telefono es requerido.")
                .NotEmpty().WithMessage("Telefono es requerido.");
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email es requerido.")
                .NotEmpty().WithMessage("Email es requerido.")
                .EmailAddress().WithMessage("Debe tener formato de correo electronico.");
            RuleFor(x => x.RazonSocial)
                .NotNull().WithMessage("Razon social es requerida.")
                .NotEmpty().WithMessage("Razon social es requerida.");
            RuleFor(x => x.FechaNacimiento)
                .NotNull().WithMessage("Fecha de nacimiento social es requerida.")
                .NotEmpty().WithMessage("Fecha de nacimiento social es requerida.")
                .Matches(@"^\d{2}/\d{2}/\d{4}").WithMessage("La fecha no tiene formato correcto");
                
        }
    }
}
