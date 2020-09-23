using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AspNetCoreMVC_ECommerceApp.Areas.Admin.ViewModels;
using ECommerce_API.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_ECommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        
        private readonly IProductRepo productRepo;

        public AdminController(IProductRepo productRepo)
        {
            
            this.productRepo = productRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}