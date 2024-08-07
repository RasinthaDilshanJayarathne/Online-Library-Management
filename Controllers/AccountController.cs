using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegisterViewModel registerView)
        {
            if (!ModelState.IsValid)
            {
                User user = new User
                {
                    Email = registerView.Email,
                    FirstName = registerView.FirstName,
                    LastName = registerView.LastName,
                    UserName = registerView.UserName,
                    Password = registerView.Password,
                    UserType = "Guest",
                    Contact_No = registerView.Contact_No
                };

                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{user.FirstName} {user.LastName} registered successfully. Please Login.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter a unique Email or Password");
                    return View(registerView);
                }

                return View();
            }
            return View(registerView);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginView)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .Where(x => (x.UserName == loginView.UserNameOrEmail || x.Email == loginView.UserNameOrEmail) && x.Password == loginView.Password)
                    .FirstOrDefault();

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.FirstName),
                        new Claim(ClaimTypes.Role, user.UserType)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("SecurePage");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            else
            {
                ViewBag.Name = HttpContext.User.FindFirst("Name")?.Value;
                return View();
            }
        }

    }
}
