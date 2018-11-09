using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly IProductRepository _product;

        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public CartController(IShoppingCartRepository shoppingCart, IProductRepository product)
        {
            _shoppingCart = shoppingCart;
            _product = product;
        }

        [Authorize]
        public IActionResult Index()
        {
            OrderViewModel = new OrderViewModel()
            {
                Order = new Models.Order()
            };

            OrderViewModel.Order.Total = 0;
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var cart = _shoppingCart.Find(c => c.ApplicationUserId == claim.Value);

            if (cart != null)
            {
                OrderViewModel.ShoppingCart = _shoppingCart.ListWithUser(claim.Value);

                foreach (var item in OrderViewModel.ShoppingCart)
                {
                    item.Product = _product.Find(item.ProductId);
                    OrderViewModel.Order.Total = OrderViewModel.Order.Total + (item.Product.Price * item.Count);
                }
            }

            return View(OrderViewModel);
        }

        public IActionResult Increase(int id)
        {
            var cart = _shoppingCart.Find(id);
            cart.Count += 1;
            _shoppingCart.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrease(int id)
        {
            var cart = _shoppingCart.Find(id);

            if (cart.Count == 1)
            {
                _shoppingCart.Delete(cart);
                var counter = _shoppingCart.Count(c => c.ApplicationUserId == cart.ApplicationUserId);
                HttpContext.Session.SetInt32("Counter", counter);
            }
            else
            {
                cart.Count -= 1;
                _shoppingCart.Save();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}