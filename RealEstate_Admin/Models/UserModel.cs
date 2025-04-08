using System.ComponentModel.DataAnnotations;

namespace RealEstate_Admin.Models
{
    public class UserModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "User name must not be empty.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User name must be between 3 and 50 characters.")]
        [RegularExpression("^[A-Za-z0-9_ ]*$", ErrorMessage = "User name must contain only letters, numbers, spaces, and underscores.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email must not be empty.")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must not be empty.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(".*[A-Z].*", ErrorMessage = "Password must contain at least one uppercase letter.")]
        //[RegularExpression(".*[a-z].*", ErrorMessage = "Password must contain at least one lowercase letter.")]
        //[RegularExpression(".*[0-9].*", ErrorMessage = "Password must contain at least one digit.")]
        //[RegularExpression(".*[@$!%*?&].*", ErrorMessage = "Password must contain at least one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role must not be empty.")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Role must contain only letters and spaces.")]
        public string Role { get; set; }

    }

    public class UserLoginModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

    }
}
