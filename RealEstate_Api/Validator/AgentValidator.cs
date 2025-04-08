using FluentValidation;
using RealEstate_Api.Models;

namespace RealEstate_Api.Validators
{
    public class AgentValidator : AbstractValidator<AgentModel>
    {
        public AgentValidator()
        {
            RuleFor(a => a.AgentName)
                .NotNull().WithMessage("Agent name must not be empty.")
                .Length(3, 100).WithMessage("Agent name must be between 3 and 100 characters.");

            RuleFor(a => a.LicenseNumber)
                .NotNull().WithMessage("License number must not be empty.")
                .Length(3, 50).WithMessage("License number must be between 3 and 50 characters.");

            RuleFor(a => a.ExperienceYears)
                .NotNull().WithMessage("Experience years must not be empty.")
                .GreaterThanOrEqualTo(0).WithMessage("Experience years must be 0 or greater.");

            RuleFor(a => a.ContactNumber)
                .NotNull().WithMessage("Contact number must not be empty.")
                .Matches(@"^\d{10,15}$").WithMessage("Contact number must be between 10 and 15 digits.");

            RuleFor(a => a.OfficeAddress)
                .MaximumLength(255).WithMessage("Office address must not exceed 255 characters.");

            RuleFor(a => a.ProfilePicture)
                .Matches("^.*\\.(jpg|jpeg|png)$").When(a => !string.IsNullOrEmpty(a.ProfilePicture))
                .WithMessage("Profile picture must be a valid image file (jpg, jpeg, png).");

            RuleFor(a => a.ProfilePicturePath)
                .Matches("^.*\\.(jpg|jpeg|png)$").When(a => !string.IsNullOrEmpty(a.ProfilePicturePath))
                .WithMessage("Profile picture path must be a valid image file (jpg, jpeg, png).");

            RuleFor(a => a.Status)
                .NotNull().WithMessage("Status must not be empty.")
                .Must(s => s == "Active" || s == "Inactive" || s == "Suspended")
                .WithMessage("Status must be 'Active', 'Inactive', or 'Suspended'.");
        }
    }
}
