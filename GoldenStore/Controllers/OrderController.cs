using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models.ViewModels;
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

        public IActionResult Index()
        {
            OrderViewModel = new OrderViewModel() {
                Order = new Models.Order()
            };

            OrderViewModel.Order.Total = 0;
            var identity = (ClaimsIdentity)this.User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var cart = _shoppingCart.Find(c => c.ApplicationUserId == claim.Value);

            if (cart != null)
            {
                OrderViewModel.ShoppingCart = _shoppingCart.ListWithUser(claim.Value);
            }

            foreach (var item in OrderViewModel.ShoppingCart)
            {
                item.Product = _product.Find(item.ProductId);
                OrderViewModel.Order.Total = OrderViewModel.Order.Total + (item.Product.Price * item.Count);
            }

            OrderViewModel.Order.CreatedAt = DateTime.Now;
            OrderViewModel.Order.UpdatedAt = DateTime.Now;

            return View(OrderViewModel);
        }
    }
}