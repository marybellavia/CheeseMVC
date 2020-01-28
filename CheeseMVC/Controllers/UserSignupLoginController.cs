using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class UserSignupLoginController : Controller
    {

        // this private field allows controller to access the database and sets up identity
        private readonly CheeseDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserSignupLoginController(CheeseDbContext dbContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            context = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserSignupLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newUser = new IdentityUser
                {
                    UserName = vm.AddUserVM.Username,
                    Email = vm.AddUserVM.Email
                };

                var result = await _userManager.CreateAsync(newUser, vm.AddUserVM.Password);

                if (result.Succeeded)
                {
                    var uzer = await _userManager.FindByNameAsync(vm.AddUserVM.Username);

                    if (uzer != null)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(uzer, vm.AddUserVM.Password, false, false);
                        if (signInResult.Succeeded)
                        {
                            return Redirect("/Category");
                        }
                    }
                }
            }
            return View("Index", vm);
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(UserSignupLoginViewModel vm)
        {

            var uzer = await _userManager.FindByNameAsync(vm.LoginUserVM.Username);

            if (uzer != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(uzer, vm.LoginUserVM.Password, false, false);
                if (signInResult.Succeeded)
                {
                    return Redirect("/Category");
                }
            }

            return View("Index", vm);
        }

        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/UserSignupLogin/Index");
        }
    }
}
