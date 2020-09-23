using ECommerce_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC_ECommerceApp.Areas.Admin.ViewModels
{
    public class ManageProductsViewModel
    {
       
        public List<Product> Products { get; set; }
    }
}
