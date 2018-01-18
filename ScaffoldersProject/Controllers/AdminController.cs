using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Data;
using ScaffoldersProject.Models;
using ScaffoldersProject.Models.services;

namespace ScaffoldersProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private IProductRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private MainDbContext _db;

        public AdminController(IProductRepository repository, UserManager<ApplicationUser> userManager,MainDbContext db)
        {
            _repository = repository;
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            //_repository.Products returns db.Products which is Products table from database
            //Includes all the products
            return View(_repository.Products);
        }

        public IActionResult NewProducts()
        {
            return View(_repository.Products);
        }

        public IActionResult Users()
        {
            return View(_userManager.Users);
        }

        public async Task<ViewResult> EditUser(string Id)
        {
            ApplicationUser userss = await _userManager.FindByIdAsync(Id);
            return View(userss);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(Users));
            }
            else
            {
                return View(user);
            }
        }
        

        public async Task<IActionResult> DeleteUser(string Id)
        {
            ApplicationUser userToBeDeleted = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(userToBeDeleted);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public IActionResult SaveProducts( IEnumerable<SaveProductsViewModel> productList)
        {
            foreach (var item in productList)
            {
                if (item.AdminApproved)
                {
                    _repository.UpdateProduct(item.Products);
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
        
        public IActionResult Delete(int productId)
        {
            var product = _repository.DeleteProduct(productId);
            return RedirectToAction(nameof(Index));
        }
    }
}