using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(string itemId, string title, string username, string imageUrl)
        {
            var model = new ReservationViewModel
            {
                UserName = username,
                ItemTitle = title,
                Reservation_Date = DateTime.Now,
                Status = "Pending",
                ImageUrl = imageUrl
            };
            Console.WriteLine(imageUrl);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel reservation)
        {
            if (!ModelState.IsValid)
            {
                // Retrieve UserId by UserName
                var userId = await _context.Users
                    .Where(u => u.Email == reservation.UserName)
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();

                // Retrieve ItemId by ItemTitle
                var itemId = await _context.Items
                    .Where(i => i.Title == reservation.ItemTitle)
                    .Select(i => i.ItemId)
                    .FirstOrDefaultAsync();

                if (userId == 0 || itemId == 0)
                {
                    ModelState.AddModelError("", "Invalid User or Item.");
                    return View(reservation);
                }

                // Save the message
                var messageEntity = new Message
                {
                    UserId = userId,
                    Content = reservation.Message,
                    Date = DateTime.Now
                };

                _context.Messages.Add(messageEntity);
                await _context.SaveChangesAsync();

                // Save the reservation
                var reservationEntity = new Reservation
                {
                    UserId = userId,
                    ItemId = itemId,
                    Reservation_Date = reservation.Reservation_Date,
                    Status = reservation.Status,
                    MessageId = messageEntity.MessageId
                };

                _context.Reservations.Add(reservationEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("AllItems", "Home");
            }
            return View(reservation);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.Reservations
                .Where(r => r.ReservationId == id)
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
                .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var reservation = await _context.Reservations
                    .FindAsync(model.ReservationId);

                if (reservation == null)
                {
                    return NotFound();
                }

                reservation.Status = model.Status;

                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction("PendingReservations", "Reservations");
            }

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;

            if (username == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = await _context.Users
                .Where(u => u.Email == username)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var reservations = await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.Item)
                .Include(r => r.Message)
                .ToListAsync();

            return View(reservations);

        }

        [HttpGet]
        public async Task<IActionResult> PendingReservations()
        {
            var reservations = await _context.Reservations
                .Where(r => r.Status == "Pending")
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
    }
}
