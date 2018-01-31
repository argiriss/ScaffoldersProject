using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models;
using ScaffoldersProject.Services;
using ScaffoldersProject.Services.CryptoObj;
using Newtonsoft.Json;

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

        public async Task<IActionResult> Index()
        {
            var send = await _webApiFetch.WebApiFetchAsync("https://api.coinmarketcap.com/v1/ticker/?convert=EUR&limit=20");
            List<Crypto> cryptos = JsonConvert.DeserializeObject<List<Crypto>>(send);      
            return View(cryptos);
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
            var send = await _webApiFetch.WebApiFetchAsync("https://api.coinmarketcap.com/v1/ticker/?convert=EUR&limit=10");
            ViewBag.send = send;
            return View();
        }
    }
}
