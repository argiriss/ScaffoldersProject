using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScaffoldersProject.Controllers
{
    public class ClientController : Controller
    {
        //private IProductRepository _repository;

        public IActionResult Index()
        {
            return View();
        }

        //Θα καλειται οταν ο client κλικαρει σε ενα προιον και θα του το εμφανιζει μονο του με τα details
        public IActionResult ViewProduct(int productId)
        {
            return View(/*_repository.Products.FirstOrDefault(p => p.ProductID == productId)*/);
        }

        public IActionResult AddToCart(int productId)
        {
            return View();
        }

    }
}