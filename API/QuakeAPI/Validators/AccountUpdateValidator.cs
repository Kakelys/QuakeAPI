using FluentValidation;
using QuakeAPI.DTO;

namespace QuakeAPI.Validators
{
    public class AccountUpdateValidator : AbstractValidator<AccountUpdate>
    {
        public AccountUpdateValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(32);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.OldPassword).NotEmpty().When(x => x.NewPassword != null);
            RuleFor(x => x.NewPassword).NotEmpty().When(x => x.OldPassword != null);
        }
    }
}