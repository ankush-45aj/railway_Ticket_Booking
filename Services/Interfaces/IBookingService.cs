using RailwayBookingApp.Models;

namespace RailwayBookingApp.Services.Interfaces
{
    public interface IBookingService
    {
        Task CreateBooking(Booking booking);
    }
}
