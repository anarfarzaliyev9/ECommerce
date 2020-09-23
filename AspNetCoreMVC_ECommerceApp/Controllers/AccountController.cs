using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMVC_ECommerceApp.Abstractions;
using AspNetCoreMVC_ECommerceApp.ViewModels;
using ECommerce_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVC_ECommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICartRepo cartRepo;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
            ICartRepo cartRepo)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cartRepo = cartRepo;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user.UserName, loginViewModel.Password, loginViewModel.RememberMe, false);
                    if (result.Succeeded)
                    {

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("Email", "User not exists");
            return View(loginViewModel);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (model.IsAgree)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                       UserName=model.Email,
                       Email=model.Email
                    };
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                       
                        await CreateCart(model.Email);
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            ModelState.AddModelError("IsAgree", "Please agree with terms of service");
            return View(model);

        }
        private async Task CreateCart(string username)
        {
            var user=await userManager.FindByNameAsync(username);
            
            Cart cart = new Cart()
            {
                UserId = user.Id
            };
            var createdCart= await cartRepo.Create(cart);
            if (createdCart != null)
            {
                user.CartId = createdCart.Id;
                await userManager.UpdateAsync(user);
            }
        }
    }
}