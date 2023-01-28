using EBusiness.Dtos.UserDtos;
using EBusiness.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EBusiness.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: AccountController/Create
        //public async Task<IActionResult> Create()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "User" });

        //    return Json("Ok");
        //}

        // GET: AccountController/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            User user = new User()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                   
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("Index","Home");
        }

        // GET: AccountController/Delete/5
        public ActionResult Login()
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(string.IsNullOrWhiteSpace(loginDto.UserName)|| string.IsNullOrWhiteSpace(loginDto.Password))
            {
                ModelState.AddModelError("", "Username or Password incorrect");
                return View();
            }

            User user = await _userManager.FindByNameAsync(loginDto.UserName);
            if(user is null)
            {
                if (string.IsNullOrWhiteSpace(loginDto.UserName) || string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    ModelState.AddModelError("", "Username or Password incorrect");
                    return View();
                }
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, true, true);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return Redirect("/admin/teammembers");
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
