using FluentValidation;
using FluentValidation.AspNetCore;
using RealEstate_Api.Models;

namespace RealEstate_Api.Validator
{
    public class PropertyTypeValidator : AbstractValidator<PropertyTypeModel>
    {
        public PropertyTypeValidator() 
        {
            RuleFor(x => x.PropertyType)
                .NotNull().WithMessage("Property type must not be empty.")
                .Matches("^[A-Za-z ]*$").WithMessage("Property type must contain only letters and spaces.");
        }
    }
}
