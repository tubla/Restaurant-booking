using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Core
{
    public class UserAudit
    {
        [Required]
        [MinLength(128), MaxLength(128)]
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [MinLength(128), MaxLength(128)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
