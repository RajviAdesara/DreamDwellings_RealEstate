using System.ComponentModel.DataAnnotations;

namespace RealEstate_Api.Models
{
    public class AppointmentModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string UserEmail { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Default value
    }

    public class AppointmentStatusUpdateModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}

