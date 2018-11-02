using GoldenStore.Interfaces;
using GoldenStore.Models;
using GoldenStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GoldenStore.Areas.Management.Controllers
{
    [Area("Management")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _category;

        [BindProperty]
        public CategoryViewModel CategoryViewModel { get; set; }

        public CategoryController(ICategoryRepository category)
        {
            _category = category;

            CategoryViewModel = new CategoryViewModel()
            {
                Categories = _category.List(),
                Category = new Category()
            };
        }

        // GET: Management/Category
        public IActionResult Index()
        {
            var categories = _category.List();
            return View(categories);
        }

        // GET: Management/Category/Create
        public IActionResult Create()
        {
            return View(CategoryViewModel);
        }

        // POST: Management/Category/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            CategoryViewModel.Category.CreatedAt = DateTime.Now;
            CategoryViewModel.Category.UpdatedAt = DateTime.Now;

            category = CategoryViewModel.Category;

            if (ModelState.IsValid)
            {
                _category.Create(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Management/Category/Update/5
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();

            CategoryViewModel.Category = _category.Find(id);
            if (CategoryViewModel == null) return NotFound();

            return View(CategoryViewModel);
        }

        // POST: Management/Category/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Category category)
        {
            category = _category.Find(id);
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                category.Name = CategoryViewModel.Category.Name;
                category.ParentId = CategoryViewModel.Category.ParentId;
                category.DisplayOrder = CategoryViewModel.Category.DisplayOrder;
                category.IsActive = CategoryViewModel.Category.IsActive;
                category.UpdatedAt = DateTime.Now;

                _category.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(CategoryViewModel);
        }

        // GET: Management/Category/Show/5
        public IActionResult Show(int? id)
        {
            if (id == null) return NotFound();

            CategoryViewModel.Category = _category.Find(id);
            if (CategoryViewModel == null) return NotFound();

            return View(CategoryViewModel);
        }

        // GET: Management/Category/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            CategoryViewModel.Category = _category.Find(id);
            if (CategoryViewModel == null) return NotFound();

            return View(CategoryViewModel);
        }

        // POST: Management/Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var category = _category.Find(id);
            _category.Delete(category);

            return RedirectToAction(nameof(Index));
        }
    }
}