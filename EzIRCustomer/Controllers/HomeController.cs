using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EzIRCustomer.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using EzIRCustomer.Authentication;
using Microsoft.Extensions.Configuration;

namespace EzIRCustomer.Controllers
{
    [RequiredLogin]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private string UserManualURL { get; }

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
            UserManualURL = _configuration.GetSection("UrlUserManual").Value;
        }

        public IActionResult Index()
        {
            //List<string> model2 = _cache.GetOrCreate("TestCache", a => {

            //    a.SetSize(1);
            //    a.SlidingExpiration = TimeSpan.FromSeconds(30);

            //    List<string> model = new List<string>();
            //    model.Add("1");
            //    model.Add("2");
            //    model.Add("3");
            //    model.Add("4");
            //    model.Add("5");
            //    model.Add("6");

            //    return model;
            //});            

            //return View(model2);
            ViewBag.userManual = UserManualURL;
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
