using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using GoldenStore.Models.ViewModels;
using GoldenStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = StaticDetails.Administrator)]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _order;
        private readonly IOrderDetailRepository _orderDetail;

        public OrderController(IOrderRepository order, IOrderDetailRepository orderDetail)
        {
            _order = order;
            _orderDetail = orderDetail;
        }

        public IActionResult Index()
        {
            List<OrderViewModel> orderViewModel = new List<OrderViewModel>();
            List<Order> orders = _order.ListOrders();

            foreach (var item in orders)
            {
                OrderViewModel ovm = new OrderViewModel();
                ovm.Order = item;
                ovm.OrderDetails = _orderDetail.ListWithOrder(item.Id);
                orderViewModel.Add(ovm);
            }

            return View(orderViewModel);
        }

        public IActionResult Confirm(int id)
        {
            Order order = _order.Find(id);
            order.Status = "Confirmed";
            _order.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reject(int id)
        {
            Order order = _order.Find(id);
            order.Status = "Rejected";
            _order.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}