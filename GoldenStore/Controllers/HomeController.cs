using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoldenStore.Models;
using GoldenStore.Interfaces;
using GoldenStore.Models.ViewModels;

namespace GoldenStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _product;
        private readonly ICategoryRepository _category;
        private readonly ICouponRepository _coupon;

        public HomeController(IProductRepository product, ICategoryRepository category, ICouponRepository coupon)
        {
            _product = product;
            _category = category;
            _coupon = coupon;
        }

        public IActionResult Index()
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                Products = _product.ListWithCategories(),
                Categories = _category.ListParentCategories(),
                Coupons = _coupon.List(),
            };
            return View(indexViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
