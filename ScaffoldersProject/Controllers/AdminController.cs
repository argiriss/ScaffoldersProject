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
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace ScaffoldersProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IProductRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private MainDbContext _db;

        public AdminController(IProductRepository repository, UserManager<ApplicationUser> userManager, MainDbContext db)
        {
            _repository = repository;
            _userManager = userManager;
            _db = db;
        }
        //_repository.Products returns db.Products which is Products table from database
        //Includes all the products

        public IActionResult Index()
        {
            //procedure to View the Image and convert the _repository.Product to View Model
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
                    MemoryStream ms = new MemoryStream(product.Image);
                    byte[] imageBytes = ms.ToArray();
                    viewImage.Image = Convert.ToBase64String(imageBytes);
                }
                viewImageList.Add(viewImage);
            }

            return View(viewImageList);
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
        public IActionResult SaveProducts(IEnumerable<SaveProductsViewModel> productList)
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
            //Turn the repo.products into view model so it can be edited
            Products product = _repository.Products.FirstOrDefault(i => i.ProductId == productId);
            var imageView = new ImageViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock,
            };

            //What about the Image?Have to check it.
            
            return View(imageView);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(ImageViewModel imageView)
        {
            //Update the changes from the Edit Form View model in the product DB
            if (ModelState.IsValid)
            {   
                var product = _repository.Products.FirstOrDefault(i => i.ProductId == imageView.ProductId);
                product.ProductId = imageView.ProductId;
                product.Name = imageView.Name;
                product.Description = imageView.Description;
                product.Category = imageView.Category;
                product.Price = imageView.Price;
                product.Stock = imageView.Stock;

                if (imageView.Image != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await imageView.Image.CopyToAsync(ms);
                        product.Image = ms.ToArray();
                        product.ContentType = imageView.Image.ContentType;
                    }
                }
                

                _repository.UpdateProduct(product);
                TempData["Message"] = $"{imageView.Name} has been updated.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(imageView);
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