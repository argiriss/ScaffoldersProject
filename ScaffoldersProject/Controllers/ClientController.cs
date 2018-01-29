using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using ScaffoldersProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System;
using System.Threading.Tasks;
using ScaffoldersProject.Services;

namespace ScaffoldersProject.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private IProductRepository _repository;
        private ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebApiFetch _webApiFetch;
        //private Cart cart;

        //Constructor depedency injection 
        public ClientController(IProductRepository repository,
            ICartRepository cartRepository,
            UserManager<ApplicationUser> userManager,
            IWebApiFetch webApiFetch)
        {
            _repository = repository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _webApiFetch = webApiFetch;

        }

        public IActionResult Index()
        {
            //Turn repo.Products into a List of View Model to return in the View
            var viewImageList = new List<ViewImageViewModel>();
            foreach (var product in _repository.Products)
            {
                var viewImage = new ViewImageViewModel
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ContentType = product.ContentType
                };

                if (product.Image != null)
                {
                    var ms = new MemoryStream(product.Image);
                    byte[] imageBytes = ms.ToArray();
                    viewImage.Image = Convert.ToBase64String(imageBytes);
                }
                viewImageList.Add(viewImage);
            }
            //HttpContext.Session.SetString("Data", "From session");//Do we need this?
            return View(viewImageList);
        }

        //Θα καλειται οταν ο client κλικαρει σε ενα προιον και θα του το εμφανιζει μονο του με τα details
        public IActionResult ViewProduct(int productId)
        {
            ViewBag.Data = HttpContext.Session.GetString("Data");
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
            //Image Procedure for the Main Product of the View
            var viewImage = new ViewImageViewModel();
            if (product.Image != null)
            {
                var ms = new MemoryStream(product.Image);
                byte[] imageBytes = ms.ToArray();
                viewImage.Image = Convert.ToBase64String(imageBytes);
            }

            List<Products> similarProducts = _repository.Products.Where(i => i.Category == product.Category).ToList();
            //Image Procedure for the Same Category Products of the view
            var similarViewImageList = new List<ViewImageViewModel>();
            foreach (var prod in similarProducts)
            {
                var similarViewImage = new ViewImageViewModel
                {
                    ProductId = prod.ProductId,
                    Name = prod.Name,
                    Description = prod.Description,
                    Price = prod.Price,
                    ContentType = prod.ContentType
                };
                if (prod.Image != null)
                {
                    var ms = new MemoryStream(prod.Image);
                    byte[] imageBytes = ms.ToArray();
                    similarViewImage.Image = Convert.ToBase64String(imageBytes);

                }
                similarViewImageList.Add(similarViewImage);
            }
            SameCategoryViewModel sameCategory = new SameCategoryViewModel(product, similarProducts, viewImage, similarViewImageList);
            return View(sameCategory);
        }

        public async Task<IActionResult> Deposit()
        {
            var send = await _webApiFetch.PaypalToken("https://api.sandbox.paypal.com/v1/oauth2/token");
            //var send = await _webApiFetch.WebApiFetchAsync("https://api.coinbase.com", "/v2/prices/spot?currency=EUR");
            ViewBag.token = send.access_token;
            ViewBag.type = send.token_type;
            return View();
        }

        public IActionResult CreatePayment()
        {
            return View();
        }

        public IActionResult ExecutePayment()
        {
            return View();
        }
    }
}