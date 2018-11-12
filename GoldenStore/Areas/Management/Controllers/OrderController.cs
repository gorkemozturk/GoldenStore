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
        private readonly IApplicationUserRepository _user;

        public OrderController(IOrderRepository order, IOrderDetailRepository orderDetail, IApplicationUserRepository user)
        {
            _order = order;
            _orderDetail = orderDetail;
            _user = user;
        }

        public IActionResult Index(string email = null, string username = null, string order = null)
        {
            List<OrderViewModel> orderViewModel = new List<OrderViewModel>();

            if (email != null || username != null || order != null)
            {
                var user = new ApplicationUser();
                List<Order> orders = new List<Order>();

                if (order != null)
                {
                    orders = _order.List(o => o.Id == Convert.ToInt32(order));
                }
                else
                {
                    if (email != null)
                    {
                        user = _user.Find(u => u.Email.ToLower().Contains(email.ToLower()));
                    }
                    else
                    {
                        if (username != null)
                        {
                            user = _user.Find(u => u.FirstName.ToLower().Contains(username.ToLower()));
                        }
                    }
                }
                if (user != null || orders.Count > 0)
                {
                    if (orders.Count == 0)
                    {
                        orders = _order.ListRelatedWithUser(user.Id);
                    }

                    foreach (Order item in orders)
                    {
                        OrderViewModel ovm = new OrderViewModel
                        {
                            Order = item,
                            OrderDetails = _orderDetail.ListWithOrder(item.Id)
                        };
                        orderViewModel.Add(ovm);
                    }
                }
            }
            else
            {
                List<Order> orders = _order.ListOrders();

                foreach (var item in orders)
                {
                    OrderViewModel ovm = new OrderViewModel
                    {
                        Order = item,
                        OrderDetails = _orderDetail.ListWithOrder(item.Id)
                    };
                    orderViewModel.Add(ovm);
                }
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