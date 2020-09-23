using ECommerce_API.Abstractions;
using ECommerce_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC_ECommerceApp.Abstractions
{
    public interface ICartRepo: IGeneralRepo<Cart>
    {
    }
}
