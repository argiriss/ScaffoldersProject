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

namespace ScaffoldersProject.Controllers
{
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
            foreach (var product in _repository.Products)
            {
                var viewImage = new ViewImageViewModel();
                viewImage.Description = product.Description;
                viewImage.Name = product.Name;
                viewImage.Price = product.Price;
                viewImage.ContentType = product.ContentType;
                
                MemoryStream ms = new MemoryStream(product.Image);
                byte[] imageBytes = ms.ToArray();
                viewImage.Image = Convert.ToBase64String(imageBytes);
                                             
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
                    Name = imageView.Name,
                    Description = imageView.Description,
                    Category = imageView.Category,
                    Price = imageView.Price,
                    Stock = imageView.Stock
                };
                //Upload Image
                using (var memoryStream = new MemoryStream())
                {
                    await imageView.Image.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                    product.ContentType = imageView.Image.ContentType;
                }
                _repository.SaveProduct(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(imageView);
            }

        }

        //[HttpGet]
        //public FileStreamResult ViewImage(int productId)
        //{
        //    Products product = _repository.Products.FirstOrDefault(m => m.ProductId == productId);
        //    MemoryStream ms = new MemoryStream(product.Image);

        //    return new FileStreamResult(ms, product.ContentType);

        //}



    }
}