using System.ComponentModel.DataAnnotations;

namespace RailwayBookingApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int TrainId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public int Seats { get; set; }
        public int? UserId { get; set; }

        public string Class { get; set; } = "Sleeper";
        public string SeatNumbers { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // ✅ New fields for History tracking
        public string Status { get; set; } = "Confirmed"; 
        public DateTime BookingDate { get; set; } = DateTime.Now;

        // ✅ Navigation property
        public Train? Train { get; set; }
    }
}