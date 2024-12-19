using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Display the registration form
        public IActionResult Create() => View();

        // Handle the registration logic
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new AppUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };

                // Create the user in the database
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    // If successful, sign the user in and redirect to the dashboard
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return Redirect("/admin/products"); // Adjust this redirect as needed
                }

                // If creation fails, add errors to the ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(user);
        }
    }
}