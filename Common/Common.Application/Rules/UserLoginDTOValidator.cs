using Application.DTOS.Entities.User;
using FluentValidation;


namespace Common.Application.Rules
{
    public class UserLoginDTOValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginDTOValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.");

            RuleFor(x => x)
                .Must(HaveValidLoginCombination)
                .WithMessage("You must provide Email and Password, or Username and Password.");

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress()
                    .WithMessage("Invalid email format.");
            });
        }

        private bool HaveValidLoginCombination(UserLoginDTO dto)
        {
            var hasEmail = !string.IsNullOrWhiteSpace(dto.Email);
            var hasUsername = !string.IsNullOrWhiteSpace(dto.Username);
            var hasPassword = !string.IsNullOrWhiteSpace(dto.Password);
            if (!hasPassword) return false;
            return (hasEmail || hasUsername);
        }
    }


}
