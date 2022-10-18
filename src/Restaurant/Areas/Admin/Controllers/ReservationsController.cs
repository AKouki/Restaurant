#nullable disable
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Areas.Admin.Models;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Data;
using Restaurant.Models;
using Restaurant.Services.Email;
using System.Globalization;

namespace Restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public ReservationsController(ApplicationDbContext db,
            IEmailService emailService,
            IMapper mapper)
        {
            _db = db;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchString,
            string sortOrder,
            string status,
            string reservationDate,
            int? pageNumber)
        {
            ViewData["SearchString"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ReservationDateSort"] = string.IsNullOrEmpty(sortOrder) ? "reservationDate_desc" : "";
            ViewData["CreationDateSort"] = sortOrder == "creationDate_asc" ? "creationDate_desc" : "creationDate_asc";
            ViewData["Status"] = status;
            ViewData["ReservationDate"] = reservationDate;

            var items = _db.Reservations.AsQueryable();

            if (DateTime.TryParse(reservationDate, out DateTime dt))
            {
                items = items.Where(r => r.ReservationDateTime.Date == dt.Date);
            }

            switch (status?.ToLower())
            {
                case "pending":
                    items = items.Where(r => r.IsConfirmed == false);
                    break;
                case "new":
                    items = items.Where(r => r.IsRead == false);
                    break;
                case "past":
                    items = items.Where(r => r.ReservationDateTime.Date < DateTime.Now.Date);
                    break;
                default:
                    items = items.Where(r => r.ReservationDateTime.Date >= DateTime.Now.Date);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                pageNumber = 1;

                items = items.Where(s => s.FirstName.Contains(searchString)
                || s.LastName.Contains(searchString)
                || s.Phone.Contains(searchString)
                || s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "reservationDate_desc":
                    items = items.OrderByDescending(r => r.ReservationDateTime);
                    break;
                case "creationDate_asc":
                    items = items.OrderBy(r => r.CreatedAt);
                    break;
                case "creationDate_desc":
                    items = items.OrderByDescending(r => r.CreatedAt);
                    break;
                default:
                    items = items.OrderBy(r => r.ReservationDateTime);
                    break;
            }

            var pageSize = 10;
            var result = await PaginatedList<Reservation>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            return PartialView(_mapper.Map<ReservationViewModel>(reservation));
        }

        public IActionResult Create()
        {
            var now = DateTime.Now;
            var model = new ReservationViewModel()
            {
                ReservationDateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0),
                Guests = 1
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newReservation = new Reservation()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    ReservationDateTime = model.ReservationDateTime,
                    Guests = model.Guests,
                    Notes = model.Notes,
                    CreatedAt = DateTime.Now,
                    IsConfirmed = true // Reservations made by Admin are confirmed by default.
                };

                _db.Reservations.Add(newReservation);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            var reservationDt = reservation.ReservationDateTime;
            reservation.ReservationDateTime = new DateTime(reservationDt.Year, reservationDt.Month, reservationDt.Day, reservationDt.Hour, reservationDt.Minute, 0);

            return View(_mapper.Map<ReservationViewModel>(reservation));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reservationToEdit = await _db.Reservations.FindAsync(id);
                if (reservationToEdit == null)
                    return NotFound();

                reservationToEdit.FirstName = model.FirstName;
                reservationToEdit.LastName = model.LastName;
                reservationToEdit.Email = model.Email;
                reservationToEdit.Phone = model.Phone;
                reservationToEdit.ReservationDateTime = model.ReservationDateTime;
                reservationToEdit.Guests = model.Guests;
                reservationToEdit.Notes = model.Notes;
                reservationToEdit.CreatedAt = DateTime.Now;
                reservationToEdit.IsConfirmed = model.IsConfirmed;
                reservationToEdit.IsRead = model.IsRead;

                _db.Update(reservationToEdit);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Admin/Reservations/{id}/Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, bool sendEmail)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.IsConfirmed = true;

            if (sendEmail && !string.IsNullOrEmpty(reservation.Email))
            {
                reservation.NotificationSentAt = DateTime.Now;

                await _emailService.SendEmailAsync(reservation.Email,
                    "Reservation Confirmation - Restaurant",
                    "<p>Your reservation has been successfully confirmed</p>");
            }

            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Admin/Reservations/{id}/Disapprove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disapprove(int id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.IsConfirmed = false;

            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Admin/Reservations/{id}/MarkAsRead")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.IsRead = true;

            _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkDelete(IEnumerable<int> reservationIds)
        {
            var reservations = await _db.Reservations
                .Where(r => reservationIds.Contains(r.Id))
                .ToListAsync();

            _db.Reservations.RemoveRange(reservations);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkApprove(IEnumerable<int> reservationIds)
        {
            var reservations = await _db.Reservations
                .Where(r => reservationIds.Contains(r.Id))
                .ToListAsync();

            foreach (var item in reservations)
            {
                item.IsConfirmed = true;
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkDisapprove(IEnumerable<int> reservationIds)
        {
            var reservations = await _db.Reservations
                .Where(r => reservationIds.Contains(r.Id))
                .ToListAsync();

            foreach (var item in reservations)
            {
                item.IsConfirmed = false;
            }

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
