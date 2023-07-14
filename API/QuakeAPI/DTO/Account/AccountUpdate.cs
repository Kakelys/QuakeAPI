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
}