using RailwayBookingApp.Data;
using RailwayBookingApp.Models;
using RailwayBookingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RailwayBookingApp.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateBooking(Booking booking)
        {
            var train = await _context.Trains.FindAsync(booking.TrainId);

            if (train == null)
                throw new InvalidOperationException("Train not found");

            if (train.AvailableSeats < booking.Seats)
                throw new InvalidOperationException("Not enough seats available");

            // ✅ Pricing rules
            decimal baseFare = 500; // Example base fare
            switch (booking.Class)
            {
                case "Sleeper":
                    booking.Price = baseFare * booking.Seats;
                    break;
                case "AC":
                    booking.Price = (baseFare * 1.5m) * booking.Seats;
                    break;
                case "FirstClass":
                    booking.Price = (baseFare * 2m) * booking.Seats;
                    break;
                default:
                    booking.Price = baseFare * booking.Seats;
                    break;
            }

            // ✅ Assign seat numbers (simple sequential example)
            booking.SeatNumbers = string.Join(", ",
                Enumerable.Range(1, booking.Seats).Select(s => $"S{train.Id}-{s}"));

            // ✅ Update seat count
            train.AvailableSeats -= booking.Seats;

            // ✅ Save booking
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }
    }
}