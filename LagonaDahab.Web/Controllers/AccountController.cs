using LagonaDahab.Application.Common.Interfaces;
using LagonaDahab.Application.Common.Utility;
using LagonaDahab.Domain.Entities;
using LagonaDahab.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LagonaDahab.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork,
               UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager,
               RoleManager<IdentityRole> roleManager)
            
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string? returnUrl = null)
        {

            // If no return URL is provided, default to the home page URL.350
            returnUrl ??= Url.Content("~/");

            // Create a new instance of LoginViewModel and set the RedirectUrl property.
            LoginViewModel model = new()
            {
                RedirectUrl = returnUrl
            };

            return View(model);
        }
        public async  Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(loginVM.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(loginVM);
                }


                // Check if the user exists in the database.
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName, 
                    loginVM.Password,
                    loginVM.RememberMe,
                    lockoutOnFailure: false
                );


                if (result.Succeeded)
                {
                    // Redirect to the return URL or a default page
                    if (!string.IsNullOrEmpty(loginVM.RedirectUrl))
                    {
                        return LocalRedirect(loginVM.RedirectUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(loginVM);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        public IActionResult Register(string? returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");
            // Check if the roles "Admin" and "Customer" exist in the database.
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                // Create the roles and seed them to the database
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
            }


            // Create a new instance of RegisterViewModel.
            RegisterViewModel model = new()
            {
                RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name

                }).ToList(),
                RedirectUrl = returnUrl

            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {


            // Check if the roles "Admin" and "Customer" exist in the database.
            registerVM.RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            // Check if the email already exists in the database.
            var existingUser =await _userManager.FindByEmailAsync(registerVM.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This email address is already registered.");
   
                return View(registerVM);
            }

            // Generate username from email
            var userName = registerVM.Email.Split('@')[0];

            // Check if the user already exists in the database.
            var existingUserName = await _userManager.FindByNameAsync(userName);
            if (existingUserName != null)
            {
                ModelState.AddModelError(string.Empty, $"The username '{userName}' is already taken." +
                    $" Please use a different email address.");
                return View(registerVM);
            }


            if (ModelState.IsValid)
            {
                ApplicationUser user = new ()
                {
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    Name = registerVM.Name,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now,
                    UserName = registerVM.Email.Split("@")[0],
                };

                //check email not exist

                // Create a new user with the provided email and password.
                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role to the user
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                        // If no role is selected, assign a default role (e.g., "Customer")
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }

                    // Sign in the user after successful registration
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect to the return URL or a default page
                    if (!string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                } 
                
                    // If registration fails, add errors to the ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                // If the user already exists, add an error to the ModelState
                if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    ModelState.AddModelError(string.Empty, "User already exists.");
                }

                registerVM.RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                });
            }
            return View(registerVM);
        }
    }
}