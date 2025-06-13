using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using RecipesApp.Data;
using RecipesApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RecipesApp.Models;

namespace RecipesApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly RecipesContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(RecipesContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogDebug("GET /Account/Register");
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {

            TempData["Debug"] = $"REGISTER form posted: Name={model.Name}, Surname={model.Surname}, Email={model.Email}";

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Register: invalid model state");
                return View(model);
            }

            try
            {
                _context.Users.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Register: error saving user {Email}", model.Email);
                ModelState.AddModelError("", "Unexpected error, please try again.");
                return View(model);
            }

            _logger.LogInformation("Register: user {Email} created with ID {Id}", model.Email, model.UserId);

            TempData["Success"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            _logger.LogDebug("GET /Account/Login (returnUrl={returnUrl})", returnUrl);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest req, string returnUrl = null)
        {

            TempData["Debug"] = $"LOGIN form posted: Email={req.Email}, Password={(string.IsNullOrEmpty(req.Password) ? "(empty)" : new string('*', req.Password.Length))}";

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login: invalid model state");
                return View(req);
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Email == req.Email && u.Password == req.Password);

            if (user == null)
            {
                _logger.LogWarning("Login failed for {Email}", req.Email);
                ModelState.AddModelError("", "Invalid email or password");
                return View(req);
            }

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );

            _logger.LogInformation("Login successful for {Email}", req.Email);
            TempData["Success"] = "Login successful!";
            return Redirect(returnUrl ?? Url.Action("Index", "Recipes"));
        }

        // POST: /Account/Logout
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogDebug("POST /Account/Logout");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Recipes");
        }

        // GET: /Account/Profile
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out var userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var vm = new ProfileViewModel
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            };
            return View(vm);
        }

        // POST: /Account/Profile
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out var userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // 4) apply edits
            user.Name = vm.Name;
            user.Surname = vm.Surname;
            if (!string.IsNullOrEmpty(vm.NewPassword))
            {
                user.Password = vm.NewPassword;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction("Profile");
        }
    }
}

