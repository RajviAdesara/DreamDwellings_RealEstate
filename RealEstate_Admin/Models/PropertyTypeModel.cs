using System.ComponentModel.DataAnnotations;

namespace RealEstate_Admin.Models
{
    public class PropertyTypeModel
    {
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "Property type must not be empty.")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Property type must contain only letters and spaces.")]
        public string PropertyType { get; set; }
    }
}
