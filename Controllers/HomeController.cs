using Microsoft.AspNetCore.Mvc;

namespace FormacionesApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "Formaciones");
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
