using AspNetCoreMVC_ECommerceApp.Abstractions;
using AspNetCoreMVC_ECommerceApp.Contexts;
using ECommerce_API.Models;
using ECommerce_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC_ECommerceApp.Repositories
{
    public class CartProductRepo : GeneralRepo<CartProduct>, ICartProductRepo
    {
        private readonly AppDbContext context;

        public CartProductRepo(AppDbContext context)
            :base(context)
        {
            this.context = context;
        }


        public async Task<bool> RemoveProductFromCart(int cartId, int productId)
        {
            var result = await context.CartProducts.FirstOrDefaultAsync(cp=>cp.CartId==cartId&& cp.ProductId==productId);

            if (result != null)
            {
                context.CartProducts.Remove(result);
                await context.SaveChangesAsync();
                return true;
            }
            return false;

        }
    }
}
