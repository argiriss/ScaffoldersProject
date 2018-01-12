using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models;
using ScaffoldersProject.Models.services;

namespace ScaffoldersProject.Controllers
{

    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private IProductRepository _repository;

        public AdminController(IProductRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.Products);
        }

        public IActionResult NewProducts()
        {
            return View(_repository.Products);
        }

        public IActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveProducts(IEnumerable<Products> productList)
        {
            foreach (var item in productList)
            {
                if (item.AdminApproved)
                {
                    _repository.UpdateProduct(item);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public ViewResult Edit(int productId)
        {
            return View(_repository.Products.FirstOrDefault(i => i.ProductId == productId));
        }

        [HttpPost]
        public IActionResult Edit(Products product)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateProduct(product);
                TempData["Message"] = $"{product.Name} has been updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View(new Products());
        }

        [HttpPost]
        public IActionResult Create(Products product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }

        }
    }
}