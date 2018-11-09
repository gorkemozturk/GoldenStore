using GoldenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        List<ShoppingCart> ListWithUser(string id);
        void RemoveCart(List<ShoppingCart> entity);
    }
}
