using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScaffoldersProject.Models.services;
using ScaffoldersProject.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

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
            return View(_repository.Products);
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
                using (var memoryStream = new MemoryStream())
                {
                    await imageView.Image.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                }
                _repository.SaveProduct(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(imageView);
            }

        }

        [HttpPost]//process to upload files
        public async Task<IActionResult> UploadFiles (List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            //file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formfile in files)
            {
                if (formfile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formfile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }
    }
}