using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstate_Admin.Models
{
    public class PropertyModel
    {
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Title must not be empty.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description must not be empty.")]
        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Property type must not be empty.")]
        public string PropertyType { get; set; } = "House";

        [Required(ErrorMessage = "Property type ID must not be empty.")]
        public int PropertyTypeId { get; set; } = 2;

        [Required(ErrorMessage = "Category must not be empty.")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Category must contain only letters and spaces.")]
        public string Category { get; set; } = "Buy";

        [Required(ErrorMessage = "Price must not be empty.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Location must not be empty.")]
        [StringLength(100, ErrorMessage = "Location must not exceed 100 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Area must not be empty.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than 0.")]
        public float Area { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Bedrooms must be 0 or greater.")]
        public int? Bedrooms { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Bathrooms must be 0 or greater.")]
        public int? Bathrooms { get; set; }

        [StringLength(200, ErrorMessage = "Amenities must not exceed 200 characters.")]
        public string Amenities { get; set; }

        //[Required(ErrorMessage = "Listed date must not be empty.")]
        //[DataType(DataType.Date)]
        //[CustomValidation(typeof(PropertyModel), nameof(ValidateListedDate))]
        //public DateTime ListedDate { get; set; }

        [Required(ErrorMessage = "Status must not be empty.")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Status must contain only letters and spaces.")]
        public string Status { get; set; } = "Sold";

        [Required(ErrorMessage = "Image name must not be empty.")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "Image path must not be empty.")]
        [RegularExpression("^.*\\.(jpg|jpeg|png)$", ErrorMessage = "Image path must be a valid image file (jpg, jpeg, png).")]
        public string ImagePath { get; set; }

        //public static ValidationResult ValidateListedDate(DateTime listedDate, ValidationContext context)
        //{
        //    if (listedDate > DateTime.Now)
        //    {
        //        return new ValidationResult("Listed date cannot be in the future.");
        //    }
        //    return ValidationResult.Success;
        //}
        public DateTime ListedDate { get; set; }

    }
}
