using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using GoldenStore.Models.ViewModels;
using GoldenStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = StaticDetails.Administrator)]
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
                Product = new Product()
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

        // GET: Management/Product/Update/5
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();

            ProductViewModel.Product = _product.Find(id);
            if (ProductViewModel.Product == null) return NotFound();

            return View(ProductViewModel);
        }

        // POST: Management/Product/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    var product = _product.Find(id);

                    if (files.Count != 0)
                    {
                        // If image or images exist(s)
                        var upload = Path.Combine(webRootPath, "images/product");
                        var newExtension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));
                        var oldExtension = product.Image.Substring(product.Image.LastIndexOf("."), product.Image.Length - product.Image.LastIndexOf("."));

                        if (System.IO.File.Exists(Path.Combine(upload, ProductViewModel.Product.Id + oldExtension)))
                        {
                            System.IO.File.Delete(Path.Combine(upload, ProductViewModel.Product.Id + oldExtension));
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, ProductViewModel.Product.Id + newExtension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        ProductViewModel.Product.Image = @"\images\product\" + ProductViewModel.Product.Id + oldExtension;
                    }
                    
                    if (ProductViewModel.Product.Image != null)
                    {
                        product.Image = ProductViewModel.Product.Image;
                    }

                    product.Name = ProductViewModel.Product.Name;
                    product.Description = ProductViewModel.Product.Description;
                    product.Price = ProductViewModel.Product.Price;
                    product.IsActive = ProductViewModel.Product.IsActive;
                    product.CategoryId = ProductViewModel.Product.CategoryId;
                    product.UpdatedAt = DateTime.Now;

                    _product.Save();
                }
                catch(Exception e)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ProductViewModel);
        }

        // GET: Management/Product/Show/5
        public IActionResult Show(int? id)
        {
            if (id == null) return NotFound();

            ProductViewModel.Product = _product.Find(id);
            if (ProductViewModel.Product == null) return NotFound();

            return View(ProductViewModel);
        }

        // GET: Management/Product/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            ProductViewModel.Product = _product.Find(id);
            if (ProductViewModel.Product == null) return NotFound();

            return View(ProductViewModel);
        }

        // POST: Management/Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Product product = _product.Find(id);

            if (product != null)
            {
                var upload = Path.Combine(webRootPath, "images/product");
                var extension = product.Image.Substring(product.Image.LastIndexOf("."), product.Image.Length - product.Image.LastIndexOf("."));
                var imagePath = Path.Combine(upload, product.Id + extension);

                if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

                _product.Delete(product);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}