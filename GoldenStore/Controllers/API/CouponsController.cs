using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoldenStore.Controllers.API
{
    [Route("api/[controller]")]
    public class CouponsController : Controller
    {
        private readonly ICouponRepository _coupon;

        public CouponsController(ICouponRepository coupon)
        {
            _coupon = coupon;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get(double total, string coupon = null)
        {
            var rtn = "";

            if (coupon == null)
            {
                rtn = total + ":E";
                return Ok(rtn);
            }

            var coupon_ = _coupon.Find(c => c.Name == coupon);

            if (coupon_ == null)
            {
                rtn = total + ":E";
                return Ok(rtn);
            }

            if (coupon_.MinimumAmount > total)
            {
                rtn = total + ":E";
                return Ok(rtn);
            }

            if (Convert.ToInt32(coupon_.CouponType) == (int)Coupon.ECouponType.Cash)
            {
                total = total - coupon_.Discount;
                rtn = total + ":S";
                return Ok(rtn);
            }
            else
            {
                if (Convert.ToInt32(coupon_.CouponType) == (int)Coupon.ECouponType.Percent)
                {
                    total = total - (total * coupon_.Discount / 100);
                    rtn = total + ":S";
                    return Ok(rtn);
                }
            }

            return Ok();
        }
    }
}
