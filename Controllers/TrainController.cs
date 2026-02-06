using Microsoft.AspNetCore.Mvc;
using RailwayBookingApp.Services.Interfaces;

namespace RailwayBookingApp.Controllers
{
    public class TrainController : Controller
    {
        private readonly ITrainService _trainService;

        public TrainController(ITrainService trainService)
        {
            _trainService = trainService;
        }

        public IActionResult Search()
        {
            // ✅ Use the same session key as AccountController
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

       [HttpPost]
public async Task<IActionResult> Results(string source, string destination)
{
    if (HttpContext.Session.GetString("User") == null)
        return RedirectToAction("Login", "Account");

    // Call your service to get trains
    var trains = await _trainService.SearchTrains(source, destination);

    // Pass the list to the Results view
    return View(trains);
}
    }
}