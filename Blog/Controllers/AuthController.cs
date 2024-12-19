using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AuthController(
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager
    ) : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

        if (result.Succeeded == false)
        {
            return View(vm);
        }

        var user = await _userManager.FindByNameAsync(vm.Username);
        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        if (isAdmin)
        {
            return RedirectToAction("Index", "Panel");
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (ModelState.IsValid == false)
        {
            return View(vm);
        }

        var user = new IdentityUser
        {
            Email = vm.Email,
            UserName = vm.Email
        };

        var result = await _userManager.CreateAsync(user, vm.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

}
