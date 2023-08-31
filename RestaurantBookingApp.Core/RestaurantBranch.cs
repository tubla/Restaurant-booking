using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Core
{
    public class RestaurantBranch
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public virtual Restaurant? Restaurant { get; set; } = null;

        public virtual ICollection<DiningTable> DiningTables { get; set; } = new List<DiningTable>();

    }
}