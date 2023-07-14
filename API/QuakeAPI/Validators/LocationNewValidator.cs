using FluentValidation;
using QuakeAPI.DTO;

namespace QuakeAPI.Validators
{
        public class LocationNewValidator : AbstractValidator<LocationNew>
    {
        private readonly string[] AllowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        public LocationNewValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MinimumLength(3).MaximumLength(64);
            RuleFor(r => r.Description).NotEmpty().MinimumLength(3).MaximumLength(4096);
            RuleFor(r => r.MaxPlayers).NotEmpty().GreaterThanOrEqualTo(2).LessThanOrEqualTo(64);
            RuleFor(r => r.Poster).NotEmpty().Must(p => AllowedExtensions.Contains(Path.GetExtension(p.FileName)));
            RuleFor(r => r.Location).NotEmpty().Must(l => Path.GetExtension(l.FileName) == ".loc");
        }
    }
}