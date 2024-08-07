using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;  // Make sure to include this

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;  // Add this

        public AdminController(AppDbContext appDbContext, IWebHostEnvironment hostEnvironment)
        {
            _context = appDbContext;
            _hostEnvironment = hostEnvironment;  // Initialize
        }

        public async Task<IActionResult> AdminDashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalBooks = await _context.Items.CountAsync(i => i.Type == "Book"),  // Adjust based on your Type property
                TotalCDs = await _context.Items.CountAsync(i => i.Type == "CD"),      // Adjust based on your Type property
                TotalGuestUsers = await _context.Users.CountAsync(u => u.UserType == "Guest") // Adjust based on your Role property
            };

            ViewBag.Name = HttpContext.User.FindFirst("Name")?.Value;
            return View(model);
        }


        public async Task<IActionResult> ViewAllItems()
        {
            var items = await _context.Items.ToListAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult AddItems()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItems(Item item)
        {
            if (!ModelState.IsValid)
            {
                if (item.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                    string extension = Path.GetExtension(item.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath, "images", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await item.ImageFile.CopyToAsync(fileStream);
                    }
                    item.ImageUrl = fileName;
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminDashboard));  // Change to a valid action
            }
            return View(item);
        }

        public async Task<IActionResult> EditItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int id, Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
                        string extension = Path.GetExtension(item.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath, "images", fileName);

                        // Save the new image file
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await item.ImageFile.CopyToAsync(fileStream);
                        }

                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(item.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, "images", item.ImageUrl);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        item.ImageUrl = fileName;
                    }

                    _context.Update(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(AdminDashboard));  // Change to a valid action
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return View(item);
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }


        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                // Delete the image if it exists
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var oldImagePath = Path.Combine(wwwRootPath, "images", item.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminDashboard));  // Change to a valid action
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Console.WriteLine("-------------------");
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
