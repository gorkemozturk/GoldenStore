using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Models.ViewModels
{
    public class CartViewModel
    {
        public List<ShoppingCart> ShoppingCart { get; set; }
        public Order Order { get; set; }
    }
}
