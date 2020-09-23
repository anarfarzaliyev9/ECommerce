using ECommerce_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce_API.Abstractions
{
    public interface IProductRepo:IGeneralRepo<Product>
    {
        Task<List<Product>> GetAllProductWithCategory();
    }
}
