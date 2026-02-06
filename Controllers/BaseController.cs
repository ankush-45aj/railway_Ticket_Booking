using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RailwayBookingApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. MUST match the key used in AccountController ("User")
            var user = HttpContext.Session.GetString("User");

            // 2. Get current route info
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // 3. Prevent Infinite Loop: Skip check if we are already on Login or Register
            bool isAuthPage = controller == "Account" && (action == "Login" || action == "Register");

            if (string.IsNullOrEmpty(user) && !isAuthPage)
            {
                // Redirect to Login if no session and not on an auth page
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }

            base.OnActionExecuting(context);
        }
    }
}