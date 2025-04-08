using FluentValidation;
using RealEstate_Api.Models;


namespace RealEstate_Api.Validator
{
    public class UserValidation : AbstractValidator<UserModel>
    {
        public UserValidation()
        {
            RuleFor(u => u.UserName)
            .NotNull().WithMessage("User name must not be empty.")
            .Length(3, 50).WithMessage("User name must be between 3 and 50 characters.")
            .Matches("^[A-Za-z0-9_ ]*$").WithMessage("User name must contain only letters, numbers, spaces, and underscores.");

            RuleFor(u => u.Email)
                .NotNull().WithMessage("Email must not be empty.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(u => u.Password)
                .NotNull().WithMessage("Password must not be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[@$!%*?&]").WithMessage("Password must contain at least one special character.");

            RuleFor(u => u.Role)
                .NotNull().WithMessage("Role must not be empty.")
                .Matches("^[A-Za-z ]*$").WithMessage("Role must contain only letters and spaces.");

            //RuleFor(u => u.DateCreated)
            //    .LessThanOrEqualTo(DateTime.Now).WithMessage("Date created cannot be in the future.");
        }
    }
}
