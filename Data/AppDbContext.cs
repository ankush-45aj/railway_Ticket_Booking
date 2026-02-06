using Microsoft.EntityFrameworkCore;
using RailwayBookingApp.Models;

namespace RailwayBookingApp.Data
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Train> Trains { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
