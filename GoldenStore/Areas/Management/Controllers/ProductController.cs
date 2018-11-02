using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Areas.Management.Controllers
{
    [Area("Management")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}