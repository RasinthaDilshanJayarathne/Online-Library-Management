using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IssueReturnController : Controller
    {
        private readonly AppDbContext _context;

        public IssueReturnController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .Where(r => r.Status == "Approved")
                .Include(r => r.User)
                .Include(r => r.Item)
                .Include(r => r.Message)
                .Select(r => new ReservationViewModel
                {
                    ReservationId = r.ReservationId,
                    UserName = r.User.UserName,
                    ItemTitle = r.Item.Title,
                    Reservation_Date = r.Reservation_Date,
                    Status = r.Status,
                    Message = r.Message.Content
                })
                .ToListAsync();

            return View(reservations);
        }

        [HttpGet]
        public async Task<IActionResult> IssueReturn(int id)
        {
            var reservation = await _context.Reservations
                .Where(r => r.ReservationId == id)
                .Include(r => r.User)
                .Include(r => r.Item)
                .FirstOrDefaultAsync();

            if (reservation == null || reservation.Status != "Approved")
            {
                return NotFound();
            }

            var model = new IssueReturnViewModel
            {
                ReservationId = reservation.ReservationId,
                UserName = reservation.User.UserName,
                ItemTitle = reservation.Item.Title,
                IssueDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IssueReturn(IssueReturnViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservation = await _context.Reservations
                    .FindAsync(model.ReservationId);

                if (reservation == null || reservation.Status != "Approved")
                {
                    return NotFound();
                }

                var issueReturn = new IssueReturn
                {
                    UserId = reservation.UserId,
                    Item_Id = reservation.ItemId,
                    Date = model.ReturnDate,
                    Type = model.Type
                };

                _context.IssueReturns.Add(issueReturn);
                reservation.Status = "Issued"; // Optional: Update reservation status if needed
                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "IssueReturn"); // Redirect to a relevant view after saving
            }

            return View(model);
        }
    }
}
