using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using GoldenStore.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _order;
        private readonly IOrderDetailRepository _orderDetail;
        private readonly IShoppingCartRepository _shoppingCart;
        private readonly IProductRepository _product;

        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

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

            OrderViewModel.ShoppingCart = _shoppingCart.ListWithUser(claim.Value);

            OrderViewModel.Order.ApplicationUserId = claim.Value;
            OrderViewModel.Order.CreatedAt = DateTime.Now;
            OrderViewModel.Order.UpdatedAt = DateTime.Now;
            OrderViewModel.Order.Status = "Pending";
            Order order = OrderViewModel.Order;

            _order.Create(order);

            foreach (var item in OrderViewModel.ShoppingCart)
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

            _shoppingCart.RemoveCart(OrderViewModel.ShoppingCart);
            _shoppingCart.Save();
            HttpContext.Session.SetInt32("Counter", 0);

            return RedirectToAction("Index", "Home");
        }
    }
}