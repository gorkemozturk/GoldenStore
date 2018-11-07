using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using GoldenStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _product;
        private readonly IShoppingCartRepository _shoppingCart;

        public ProductController(IProductRepository product, IShoppingCartRepository shoppingCart)
        {
            _product = product;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Show(int? id)
        {
            if (id == null) return NotFound();
            var product = _product.FindWithCategory(id);

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = product,
                ProductId = product.Id
            };

            return View(shoppingCart);
        }

        [Authorize]
        [HttpPost, ActionName("Show")]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;

            if (ModelState.IsValid)
            {
                var identity = (ClaimsIdentity)this.User.Identity;
                var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;
                shoppingCart.CreatedAt = DateTime.Now;
                shoppingCart.UpdatedAt = DateTime.Now;

                ShoppingCart cart = _shoppingCart.Find(c => c.ApplicationUserId == shoppingCart.ApplicationUserId && c.ProductId == shoppingCart.ProductId);

                if (cart == null)
                {
                    // Any products does not exists.
                    _shoppingCart.Add(shoppingCart);
                }
                else
                {
                    // Product(s) exist(s) in the shopping cart. Therefore this/those can be increased.
                    cart.Count = cart.Count + shoppingCart.Count;
                    shoppingCart.UpdatedAt = DateTime.Now;
                }

                _shoppingCart.Save();
                var counter = _shoppingCart.Count(c => c.ApplicationUserId == shoppingCart.ApplicationUserId);
                HttpContext.Session.SetInt32("Counter", counter);

                return RedirectToAction(nameof(Show));
            }
            else
            {
                var product = _product.FindWithCategory(shoppingCart.ProductId);

                ShoppingCart sCart = new ShoppingCart()
                {
                    Product = product,
                    ProductId = product.Id
                };

                return View(sCart);
            }
        }
    }
}