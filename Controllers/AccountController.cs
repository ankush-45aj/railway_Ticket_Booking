using Microsoft.AspNetCore.Mvc;
using RailwayBookingApp.Data;
using RailwayBookingApp.Models;

namespace RailwayBookingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Login page
        public IActionResult Login() => View();

        // POST: Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            // ✅ Set session values
            HttpContext.Session.SetString("User", user.Email);
            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("Dashboard", "Account");
        }

        // GET: Register page
        public IActionResult Register() => View();

        // POST: Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetString("User", user.Email);
            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("Dashboard", "Account");
        }

        // GET: Dashboard
        // GET: Dashboard
public IActionResult Dashboard()
{
    var userId = HttpContext.Session.GetInt32("UserId");
    var userEmail = HttpContext.Session.GetString("User");

    if (userId == null)
        return RedirectToAction("Login");

    ViewBag.UserId = userId;
    ViewBag.UserEmail = userEmail;

    return View();
}

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}