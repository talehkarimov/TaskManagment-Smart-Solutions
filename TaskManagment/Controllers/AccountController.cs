using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagment.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagment.Services.Abstract;
using TaskManagment.ViewModel;

namespace TaskManagment.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly IOrganizationService _organizationService;

        public AccountController(SignInManager<ApplicationUser> signInManager,
           IUserService userService,
           ITaskService taskService,
           IOrganizationService organizationService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _taskService = taskService;
            _organizationService = organizationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View("Login", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userService.GetByUsernameAsync(model.Username);
                if (user == null || !_userService.UserCanLogin(model.Username))
                {
                    ModelState.AddModelError("HasNotAccessToLoginError", "Giriş gadağandır.");
                    return View("Login", model);
                }
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);
                if (result.Succeeded)
                {
                    _userService.SetUserLastLoginDate(user.Id);
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);


                    IList<string> userRoles = await _userService.GetUserRolesAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("InvalidCredentialsError", "Email və ya şifrə yalnışdır.");
                return View(model);
            }
            return View("Login", model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            if (User.IsInRole(SystemRoles.EmployeeRole))
            {
                return NotFound();
            }
            List<Organization> organizations = _organizationService.GetAll();
            AccountViewModel viewModel = new AccountViewModel
            {
                Organizations = organizations
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(AccountViewModel viewModel)
        {
            bool success = false;
            string role = string.Empty;
            if (viewModel == null) viewModel = new AccountViewModel();
            ApplicationUser currentUser = _userService.GetAuthorizedUserAsync(User).GetAwaiter().GetResult();

            var currentUserRole = _userService.GetUserRolesAsync(currentUser).GetAwaiter().GetResult();
            if (currentUserRole != null && currentUserRole[0] == "Admin")
            {
                role = SystemRoles.EmployeeRole;
            }
            else
            {
                role = SystemRoles.OrganizationRole;
            }

            success = await _userService.AddUserWithRoleAsync(viewModel, role);


            if (success && currentUserRole != null && currentUserRole[0] == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (success)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout(string id)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
