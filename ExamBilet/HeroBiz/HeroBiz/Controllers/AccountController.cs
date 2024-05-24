using Core.DTOs.AccountDto;
using Core.Models;
using HeroBiz.Helpers.Account;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeroBiz.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

    
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            User user = new User()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Surname = registerDto.Surname,
                UserName = registerDto.UserName,
            };
            var result= await _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
            //await _userManager.AddToRoleAsync(user,UserRole.Admin.ToString());
            //await _signInManager.SignInAsync(user,isPersistent: false);
            return RedirectToAction("Index","Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
            if(user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError("", "password ve ya username yanlisdir");
                    return View();
                }
            }
            var result =  await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "birazdan yeniden cehd edin");
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "password ve ya username yanlisdir");
                return View();
            }
            await _signInManager.SignInAsync(user, loginDto.IsRemember);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = item.ToString(),
                });
                

            }
            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
