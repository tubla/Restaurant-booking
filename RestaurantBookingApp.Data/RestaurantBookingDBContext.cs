using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Core;

namespace RestaurantBookingApp.Data
{
    public class RestaurantBookingDBContext : DbContext
    {
        public RestaurantBookingDBContext(DbContextOptions<RestaurantBookingDBContext> options) : base(options)
        {

        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantBranch> RestaurantBranches { get; set; }
        public DbSet<DiningTable> DiningTables { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CustomerContactUploads> CustomerContactUploads { get; set; }
    }
}