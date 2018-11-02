using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models.ViewModels;
using GoldenStore.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Areas.Management.Controllers
{
    [Area("Management")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _product;
        private readonly ICategoryRepository _category;
        private readonly IHostingEnvironment _hostingEnvironment;

        [BindProperty]
        public ProductViewModel ProductViewModel { get; set; }

        public ProductController(IProductRepository product, ICategoryRepository category, IHostingEnvironment hostingEnvironment)
        {
            _product = product;
            _category = category;
            _hostingEnvironment = hostingEnvironment;

            ProductViewModel = new ProductViewModel()
            {
                Categories = _category.List(),
                Product = new Models.Product()
            };
        }

        // GET: Management/Product
        public IActionResult Index()
        {
            var products = _product.ListWithCategories();
            return View(products);
        }

        // GET: Management/Product/Create
        public IActionResult Create()
        {
            return View(ProductViewModel);
        }

        // POST: Management/Product/Store
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Store()
        {
            if (!ModelState.IsValid) return View(ProductViewModel);

            ProductViewModel.Product.CreatedAt = DateTime.Now;
            ProductViewModel.Product.UpdatedAt = DateTime.Now;

            _product.Create(ProductViewModel.Product);

            // Image Saving Process
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var product = _product.Find(ProductViewModel.Product.Id);

            if (files.Count != 0)
            {
                // If image or images exist(s)
                var upload = Path.Combine(webRootPath, "images/product");
                var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                using (var fileStream = new FileStream(Path.Combine(upload, ProductViewModel.Product.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                product.Image = @"\images\product\" + ProductViewModel.Product.Id + extension;
            }
            else
            {
                // If any images does not exist
                var upload = Path.Combine(webRootPath, @"images\product\" + StaticDetails.DefaultImage);
                System.IO.File.Copy(upload, webRootPath + @"\images\product\" + ProductViewModel.Product.Id + ".png");
                product.Image = @"\images\product\" + ProductViewModel.Product.Id + ".png";
            }

            _product.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}