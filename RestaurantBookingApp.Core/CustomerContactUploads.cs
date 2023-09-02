using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Core
{
    public class CustomerContactUploads : UserAudit
    {
        public int Id { get; set; }
        [Required]
        public string FilePath { get; set; } = string.Empty;
        public bool IsProcessed { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }
}
