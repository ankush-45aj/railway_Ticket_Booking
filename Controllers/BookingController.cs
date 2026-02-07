using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RailwayBookingApp.Data;
using RailwayBookingApp.Models;
using RailwayBookingApp.Services;
using RailwayBookingApp.Services.Interfaces;

namespace RailwayBookingApp.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly AppDbContext _context;
        private readonly EmailService _emailService; // ✅ Inject Email Service

        public BookingController(
            IBookingService bookingService,
            AppDbContext context,
            EmailService emailService)
        {
            _bookingService = bookingService;
            _context = context;
            _emailService = emailService;
        }

        // GET: Booking/Book
        public IActionResult Book()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Trains = new SelectList(_context.Trains, "Id", "TrainName");
            return View();
        }

        // ✅ POST: Booking/Book WITH EMAIL SENDING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(Booking booking)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.Trains = new SelectList(_context.Trains, "Id", "TrainName");
                return View(booking);
            }

            // Save booking
            booking.UserId = userId.Value;
            await _bookingService.CreateBooking(booking);

            // ✅ Fetch user + train details
            var user = await _context.Users.FindAsync(userId.Value);
            var train = await _context.Trains.FindAsync(booking.TrainId);

            // Safety check (avoid null crash)
            if (user != null && train != null)
            {
                string route = $"{train.Source} → {train.Destination}";

                try
                {
                    await _emailService.SendTicketEmail(
                        user.Email,
                        booking.PassengerName,
                        train.TrainName,
                        route,
                        booking.Seats.ToString(),
                        booking.TotalPrice.ToString()
                    );
                }
                catch (Exception ex)
                {
                    // Log error but DO NOT break booking flow
                    Console.WriteLine("Email failed: " + ex.Message);
                }
            }

            return View("Success", booking);
        }

        // GET: Booking/History
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Train)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }
    }
}
