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
using ScaffoldersProject.Services.PaypalObj;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ScaffoldersProject.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private IProductRepository _repository;
        private ICartRepository _cartRepository;
        private IWalletRepository _walletRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebApiFetch _webApiFetch;
        private static string token;
        private static string paymentId;
        //private Cart cart;

        //Constructor depedency injection 
        public ClientController(IProductRepository repository,
            ICartRepository cartRepository,
            UserManager<ApplicationUser> userManager,
            IWebApiFetch webApiFetch,
            IWalletRepository walletRepository)
        {
            _repository = repository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _webApiFetch = webApiFetch;
            _walletRepository = walletRepository;
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
            token = send.access_token;          
            return View();
        }
        
        //Don't FORGET..... parameter same name witn parameter request name ..?amt=100
        public async Task<IActionResult> CreatePayment(string amt)
        {
            var res = await _webApiFetch.CreatePayment(token,amt);
            DataContractJsonSerializer reqId = new DataContractJsonSerializer(typeof(PaymentIdRes));
            PaymentIdRes resId = new PaymentIdRes();
            resId.id = res.id;
            paymentId = res.id;
            MemoryStream ms = new MemoryStream();
            reqId.WriteObject(ms, resId);
            byte[] json = ms.ToArray();
            ms.Close();
            string jsonToSend= Encoding.UTF8.GetString(json, 0, json.Length);

            return Content(jsonToSend);
        }

        public async Task<IActionResult> ExecutePayment(string payerId)
        {
            var res = await _webApiFetch.ExecutePayment(token, paymentId, payerId);
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

        public async Task<IActionResult> DepositHistory()
        {
            var userId =  _userManager.GetUserId(User);
            var depositHistoryTable = _walletRepository.GetDepositHistory(userId);
            //depositHistoryTable.Sort();
            return View(depositHistoryTable);
        }
    }
}