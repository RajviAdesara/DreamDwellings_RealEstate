using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Api.Models
{
    public class AgentModel
    {
        public int AgentId { get; set; }

        [Required]
        [StringLength(100)]
        public string AgentName { get; set; }

        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; }

        [Required]
        public int ExperienceYears { get; set; }

        [Required]
        [StringLength(15)]
        public string ContactNumber { get; set; }

        [StringLength(255)]
        public string OfficeAddress { get; set; }

        public string ProfilePicture { get; set; }

        public string ProfilePicturePath { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("Active|Inactive|Suspended", ErrorMessage = "Invalid Status")]
        public string Status { get; set; }
    }
}
