using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMVC_ECommerceApp.Areas.Admin.ViewModels;
using AspNetCoreMVC_ECommerceApp.Extensions;
using AutoMapper;
using ECommerce_API.Abstractions;
using ECommerce_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_ECommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment webHost;
        private readonly IMapper mapper;
        private readonly IProductRepo productRepo;
        private readonly ICategoryRepo categoryRepo;

        public ProductsController(IWebHostEnvironment webHost,IMapper mapper,
           IProductRepo productRepo,ICategoryRepo categoryRepo)
        {
            this.webHost = webHost;
            this.mapper = mapper;
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
            
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            AddProductViewModel model = new AddProductViewModel();
            model.Categories = await  categoryRepo.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(
        [Bind("Name","FormFile","Price","Details", "CategoryId","Categories", "IsFeatured", "IsNewArrival")]AddProductViewModel model)
        {         
            if (ModelState.IsValid)
            {
                Product product = new Product();
                //Create fileName for Photo by using SaveAsync() extension method 
                var fileName = await model.FormFile.SaveAsync(webHost.WebRootPath, "ProductImages");
                model.PhotoPath = fileName;
                // Map properties of viewModel to product
                mapper.Map(model,product);
                
                //After mapping create product
                var result= await productRepo.Create(product);
                if (result != null)
                {
                    return RedirectToAction("ManageProducts", "Products");
                }
            }
            model.Categories = await categoryRepo.GetAll();            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            //Get category list
            List<Category> categories = await categoryRepo.GetAll();
            //Get product by id which come from view 
            Product productById = await productRepo.GetById(id);
            //Find category of product
            productById.Category = categories.FirstOrDefault(c=>c.CategoryId==productById.CategoryId);
            //Create corresponding view model
            EditProductViewModel model = new EditProductViewModel();
            //Map data
            mapper.Map(productById, model);
            //Fill categories of view model
            model.Categories = categories;
            model.ProductId = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                mapper.Map(model, product);

                var result= await productRepo.Edit(product);
                if (result)
                {
                    return RedirectToAction("ManageProducts", "Products");
                }
            }
            model.Categories = await categoryRepo.GetAll();
            return View(model);
        }
        public async Task<IActionResult> ManageProducts()
        {
            ManageProductsViewModel model = new ManageProductsViewModel();
            model.Products = await productRepo.GetAllProductWithCategory();
            return View(model);
        }
       
        public async Task<JsonResult> DeleteProduct(int id)
        {

            try
            {
                await productRepo.Delete(id);
            }
            catch (Exception e)
            {
                return Json(400);
            }

            return Json(200);
        }

       
    }
}