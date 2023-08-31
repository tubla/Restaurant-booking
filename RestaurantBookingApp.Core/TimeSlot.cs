using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Core
{

    public class TimeSlot
    {
        public int Id { get; set; }

        public int DiningTableId { get; set; }

        [Required]
        public DateTime ReservationDay { get; set; }

        [Required]
        public string MealType { get; set; } = null!;

        [Required]
        public string TableStatus { get; set; } = null!;

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
