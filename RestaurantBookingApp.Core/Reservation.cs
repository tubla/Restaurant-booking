namespace RestaurantBookingApp.Core
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationStatus { get; set; } = null!;
        public virtual TimeSlot TimeSlot { get; set; } = null!;
        public virtual User User { get; set; } = null!;

    }
}
