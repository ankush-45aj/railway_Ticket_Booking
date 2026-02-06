using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RailwayBookingApp.Data;
using RailwayBookingApp.Models;
using RailwayBookingApp.Services.Interfaces;

namespace RailwayBookingApp.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly AppDbContext _context;

        public BookingController(IBookingService bookingService, AppDbContext context)
        {
            _bookingService = bookingService;
            _context = context;
        }

        // GET: Booking/Book
        // Check session immediately so unauthorized users can't see the form
        public IActionResult Book()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Trains = new SelectList(_context.Trains, "Id", "TrainName");
            return View();
        }

        // POST: Booking/Book
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ Security best practice
        public async Task<IActionResult> Book(Booking booking)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Trains = new SelectList(_context.Trains, "Id", "TrainName");
                return View(booking);
            }

            booking.UserId = userId.Value;
            await _bookingService.CreateBooking(booking);

            return View("Success", booking);
        }

        // GET: Booking/History
        public async Task<IActionResult> History()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Train)
                .ToListAsync();

            return View(bookings);
        }
    }
}