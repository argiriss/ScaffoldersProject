using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models;
using ScaffoldersProject.Services;

namespace ScaffoldersProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebApiFetch _webApiFetch;

        //Depedency injection
        public HomeController(IWebApiFetch webApiFetch)
        {
            _webApiFetch = webApiFetch;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Test()
        {
            //var send = await _webApiFetch.PaypalToken("https://api.sandbox.paypal.com/v1/oauth2/token");
            var send = await _webApiFetch.WebApiFetchAsync("https://api.coinbase.com/v2/prices/spot?currency=EUR");
            ViewBag.type = send;
            return View();
        }
    }
}
