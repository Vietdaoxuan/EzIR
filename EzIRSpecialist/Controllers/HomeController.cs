using CoreLib.Configs;
using EzIRSpecialist.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers
{
    public class HomeController : Controller
    {
        [EzAuthorize(RoleCode.HOME, view: true)]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
