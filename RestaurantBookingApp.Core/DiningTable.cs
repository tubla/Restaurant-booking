using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Core
{
    public class DiningTable
    {
        public int Id { get; set; }

        public int RestaurantBranchId { get; set; }

        [MaxLength(100)]
        public string? TableName { get; set; }

        [Required]
        public int Capacity { get; set; }

        public virtual RestaurantBranch RestaurantBranch { get; set; } = null!;
        public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    }
}
