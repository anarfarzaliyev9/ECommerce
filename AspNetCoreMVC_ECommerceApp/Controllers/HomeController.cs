using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCoreMVC_ECommerceApp.Models;

using AspNetCoreMVC_ECommerceApp.ViewModels;
using ECommerce_Models;
using ECommerce_API.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AspNetCoreMVC_ECommerceApp.Abstractions;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace AspNetCoreMVC_ECommerceApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepo productRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICartRepo cartRepo;
        private readonly ICartProductRepo cartProductRepo;

        public HomeController(ILogger<HomeController> logger, IProductRepo productRepo,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ICartRepo cartRepo, ICartProductRepo cartProductRepo)
        {
            _logger = logger;
            this.productRepo = productRepo;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cartRepo = cartRepo;
            this.cartProductRepo = cartProductRepo;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await productRepo.GetAllProductWithCategory();
            IndexViewModel model = new IndexViewModel();
            

            if (signInManager.IsSignedIn(User))
            {
                var id = userManager.GetUserId(User);

                var users = await userManager.Users.Include(c => c.Cart).ToListAsync();
                var user = users.FirstOrDefault(u => u.Id == id);
                model.CartId = user.Cart.Id;
                var allCartProducts = await cartProductRepo.GetAll();
                //Define which products is in cart
                foreach (var product in products)
                {
                    if (allCartProducts.Any(cp => cp.ProductId == product.Id))
                    {
                        product.IsInCart = true;
                    }
                }
                //var userCartProducts = allCartProducts.Where(cp=>cp.CartId==user.CartId).ToList();
                // Filter cart products by user's id
                var filteredCartProducts = (from u in users
                                            join cp in allCartProducts
                                            on u.CartId equals cp.CartId
                                            select cp).ToList();
                var userCartProducts = (from p in products
                                        join fcp in filteredCartProducts
                                        on p.Id equals fcp.ProductId
                                        select p).ToList();
                model.UserCartProducts = userCartProducts;

            }
            
            model.FeaturedProducts = products.Where(p => p.IsFeatured == true).ToList();
            model.NewArrivalProducts = products.Where(p => p.IsNewArrival == true).ToList();
            return View(model);
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> GetProductsByCategory(string categoryName)
        {
            //Send categoryname to view for breadcrumb
            ViewBag.CategoryName = categoryName;
            //Get all products
            List<Product> products = await productRepo.GetAllProductWithCategory();
            GetByCategoryViewModel model = new GetByCategoryViewModel();
            //Fill view model produts
            model.Products = products.Where(p => string.Equals(p.Category.Name, categoryName, StringComparison.OrdinalIgnoreCase)).ToList();
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<JsonResult> AddToCart(int productId, int CartId)
        {
            CartProduct cartProduct = new CartProduct()
            {
                CartId = CartId,
                ProductId = productId
            };

            try
            {
                var result = await cartProductRepo.Create(cartProduct);
                if (result != null)
                {

                    return Json(200);
                }
            }

            catch (Exception){}

            return Json(400);
        }
        public async Task<JsonResult> RemoveFromCart(int productId, int CartId)
        {


            try
            {
               
                var result = await cartProductRepo.RemoveProductFromCart(CartId, productId);
                if (result)
                {
                    return Json(200);
                }
            }

            catch (Exception) { }

            return Json(400);
        }
    }
}
