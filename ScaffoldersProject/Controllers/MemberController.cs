using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using ScaffoldersProject.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using ScaffoldersProject.Data;
using Microsoft.AspNetCore.Authorization;

namespace ScaffoldersProject.Controllers
{
    [Authorize(Roles ="Member")]
    public class MemberController : Controller
    {
        private IProductRepository _repository;

        //Dependency Injection
        public MemberController(IProductRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {

            var viewImageList = new List<ViewImageViewModel>();
            //convert the Products into readable type for the Model
            foreach (var product in _repository.Products)
            {
                var viewImage = new ViewImageViewModel
                {
                    ProductId = product.ProductId,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                    ContentType = product.ContentType
                };

                if (product.Image != null )
                {                              
                    MemoryStream ms = new MemoryStream(product.Image);
                    byte[] imageBytes = ms.ToArray();
                    viewImage.Image = Convert.ToBase64String(imageBytes);
                }

                viewImageList.Add(viewImage);
            }
            return View(viewImageList);
        }

        public ViewResult Create()
        {
            return View(new ImageViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImageViewModel imageView)
        {
            if (ModelState.IsValid)
            {
                var product = new Products
                {
                    ProductId = imageView.ProductId,
                    Name = imageView.Name,
                    ShortName=imageView.ShortName,
                    Description = imageView.Description,
                    Category = imageView.Category,
                    Price = imageView.Price,
                    Stock = imageView.Stock
                };
                //Upload Image
                if (imageView.Image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageView.Image.CopyToAsync(memoryStream);
                        product.Image = memoryStream.ToArray();
                        product.ContentType = imageView.Image.ContentType;
                    }
                }
                _repository.SaveProduct(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(imageView);
            }

        }

        public ViewResult Edit (int productId)
        {
            //Turn the repo.products into view model so it can be edited
            Products product = _repository.Products.FirstOrDefault(i => i.ProductId == productId);
            var imageView = new ImageViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                ShortName=product.ShortName,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
            return View(imageView);
        }
        

        [HttpPost]
        public async Task<IActionResult> Edit (ImageViewModel imageView)
        {
            //Update the changes from the Edit Form View model in the product DB
            if (ModelState.IsValid)
            {
                var product = _repository.Products.FirstOrDefault(i => i.ProductId == imageView.ProductId);
                product.ProductId = imageView.ProductId;
                product.Name = imageView.Name;
                product.ShortName = imageView.ShortName;
                product.Description = imageView.Description;
                product.Category = imageView.Category;
                product.Price = imageView.Price;
                product.Stock = imageView.Stock;

                if (imageView.Image !=null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageView.Image.CopyToAsync(memoryStream);
                        product.Image = memoryStream.ToArray();
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
    }
}