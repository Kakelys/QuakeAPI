using FluentValidation;
using FluentValidation.Validators;

namespace QuakeAPI.DTO
{
    public class AccountUpdate
    {
        public string Username {get;set;} = null!;
        public string Email {get;set;} = null!;
        public string? OldPassword {get;set;}
        public string? NewPassword {get;set;}

    }

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