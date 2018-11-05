using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using GoldenStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = StaticDetails.Administrator)]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _coupon;

        public CouponController(ICouponRepository coupon)
        {
            _coupon = coupon;
        }

        // GET: Management/Coupon/
        public IActionResult Index()
        {
            var coupons = _coupon.List();
            return View(coupons);
        }

        // GET: Management/Coupon/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Management/Coupon/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Coupon coupon)
        {
            coupon.CreatedAt = DateTime.Now;
            coupon.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _coupon.Create(coupon);
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        // GET: Management/Coupon/Update/5
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();

            var coupon = _coupon.Find(id);
            if (coupon == null) return NotFound();

            return View(coupon);
        }

        // POST: Management/Coupon/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Coupon coupon)
        {
            coupon.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _coupon.Update(coupon);
                return RedirectToAction(nameof(Index));
            }

            return View(coupon);
        }

        // GET: Management/Coupon/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var coupon = _coupon.Find(id);
            if (coupon == null) return NotFound();

            return View(coupon);
        }

        // POST: Management/Coupon/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var coupon = _coupon.Find(id);
            _coupon.Delete(coupon);

            return RedirectToAction(nameof(Index));
        }
    }
}