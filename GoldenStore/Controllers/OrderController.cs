using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using GoldenStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _order;
        private readonly IOrderDetailRepository _orderDetail;
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly IProductRepository _product;

        [BindProperty]
        public CartViewModel CartViewModel { get; set; }

        public OrderController(IOrderRepository order, IOrderDetailRepository orderDetail, IShoppingCartRepository shoppingCart, IProductRepository product)
        {
            _order = order;
            _orderDetail = orderDetail;
            _shoppingCart = shoppingCart;
            _product = product;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            CartViewModel.ShoppingCart = _shoppingCart.ListWithUser(claim.Value);

            CartViewModel.Order.ApplicationUserId = claim.Value;
            CartViewModel.Order.CreatedAt = DateTime.Now;
            CartViewModel.Order.UpdatedAt = DateTime.Now;
            CartViewModel.Order.Status = "Pending";
            Order order = CartViewModel.Order;

            _order.Create(order);

            foreach (var item in CartViewModel.ShoppingCart)
            {
                item.Product = _product.Find(item.ProductId);
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };
                _orderDetail.Add(orderDetail);
            }

            _shoppingCart.RemoveCart(CartViewModel.ShoppingCart);
            _shoppingCart.Save();
            HttpContext.Session.SetInt32("Counter", 0);

            return RedirectToAction(nameof(Show), new { id = order.Id });
        }

        public IActionResult Show(int id)
        {
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            OrderViewModel orderViewModel = new OrderViewModel()
            {
                Order = _order.Find(o => o.Id == id && o.ApplicationUserId == claim.Value),
                OrderDetails = _orderDetail.ListWithOrder(id)
            };

            return View(orderViewModel);
        }
    }
}