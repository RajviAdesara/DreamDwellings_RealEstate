using FluentValidation;
using RealEstate_Api.Models;

namespace RealEstate_Api.Validator
{
    public class PropertyValidator : AbstractValidator<PropertyModel>
    {
        public PropertyValidator() 
        {
            RuleFor(p => p.Title)
                .NotNull().WithMessage("Title must not be empty.")
                .Length(5, 100).WithMessage("Title must be between 5 and 100 characters.");
            RuleFor(p => p.Description)
                .NotNull().WithMessage("Description must not be empty.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.PropertyTypeId)
                .NotNull().WithMessage("Property type must not be empty.");

            RuleFor(p => p.Category)
                .NotNull().WithMessage("Category must not be empty.")
                .Matches("^[A-Za-z ]*$").WithMessage("Category must contain only letters and spaces.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(p => p.Location)
                .NotNull().WithMessage("Location must not be empty.")
                .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

            RuleFor(p => p.Area)
                .GreaterThan(0).WithMessage("Area must be greater than 0.");

            RuleFor(p => p.Bedrooms)
                .GreaterThanOrEqualTo(0).When(p => p.Bedrooms.HasValue).WithMessage("Bedrooms must be 0 or greater.");

            RuleFor(p => p.Bathrooms)
                .GreaterThanOrEqualTo(0).When(p => p.Bathrooms.HasValue).WithMessage("Bathrooms must be 0 or greater.");

            RuleFor(p => p.Amenities)
                .MaximumLength(200).WithMessage("Amenities must not exceed 200 characters.");

            RuleFor(p => p.ListedDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Listed date cannot be in the future.");

            RuleFor(p => p.Status)
                .NotNull().WithMessage("Status must not be empty.")
                .Matches("^[A-Za-z ]*$").WithMessage("Status must contain only letters and spaces.");

            RuleFor(p => p.ImagePath)
                .NotNull().WithMessage("Image path must not be empty.")
                .Matches("^.*\\.(jpg|jpeg|png)$").WithMessage("Image path must be a valid image file (jpg, jpeg, png.");
        }
    }
}
