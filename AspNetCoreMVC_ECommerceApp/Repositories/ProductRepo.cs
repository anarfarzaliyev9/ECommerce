using AspNetCoreMVC_ECommerceApp.Contexts;
using ECommerce_API.Abstractions;
using ECommerce_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce_API.Models
{
    public class ProductRepo:GeneralRepo<Product>,IProductRepo
    {
        private readonly AppDbContext context;

        public ProductRepo(AppDbContext context)
            :base(context)
        {
            this.context = context;
        }

        public async Task<List<Product>> GetAllProductWithCategory()
        {
            return await context.Products.Include(p => p.Category).ToListAsync();
        }
        
    }
}
