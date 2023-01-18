using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Restaurant.Data;
using Restaurant.Services.ReCaptcha;
using Restaurant.ViewModels;
using System.Globalization;

namespace Restaurant.Pages.Reservation
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IReCaptchaService _reCaptchaService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public IndexModel(ApplicationDbContext db,
            IConfiguration configuration,
            IReCaptchaService reCaptchaService,
            IStringLocalizer<SharedResource> localizer)
        {
            _db = db;
            _configuration = configuration;
            _reCaptchaService = reCaptchaService;
            _localizer = localizer;
        }

        [BindProperty]
        public ReservationViewModel Reservation { get; set; } = new();

        public string? ReCaptchaSiteKey { get; set; }
        public List<SelectListItem> OpeningHours { get; set; } = new();

        public void OnGet()
        {
            ReCaptchaSiteKey = _configuration["ReCaptcha:SiteKey"];
            PopulateOpeningHours(12, 23);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReCaptchaSiteKey = _configuration["ReCaptcha:SiteKey"];
            PopulateOpeningHours(12, 23);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!await _reCaptchaService.IsValid(Reservation.Token!, HttpContext.Connection.RemoteIpAddress!.ToString(), "reservation"))
            {
                ModelState.AddModelError(string.Empty, "Invalid Captcha.");
                return Page();
            }

            if (_db.Reservations.Any(r => r.Phone == Reservation.Phone))
            {
                ModelState.AddModelError(string.Empty, _localizer["ReservationExists", _configuration["SiteSettings:ContactPhone1"] ?? ""]);
                return Page();
            }

            var newReservation = new Models.Reservation()
            {
                FirstName = Reservation.FirstName,
                LastName = Reservation.LastName,
                Email = Reservation.Email,
                Phone = Reservation.Phone,
                ReservationDateTime = Reservation.ReservationDate.Date.Add(Reservation.ReservationTime),
                Guests = Reservation.Guests,
                Notes = Reservation.Note,
                CreatedAt = DateTime.Now
            };

            _db.Reservations.Add(newReservation);
            await _db.SaveChangesAsync();

            TempData["Success"] = true;
            return RedirectToPage("Success", new { culture = CultureInfo.CurrentCulture.Name });
        }
        private void PopulateOpeningHours(int openAM, int closePM)
        {
            for (int i = openAM; i <= closePM; i++)
            {
                OpeningHours.Add(new SelectListItem() { Value = $"{i}:00", Text = $"{i}:00" });
                if (i < closePM)
                    OpeningHours.Add(new SelectListItem() { Value = $"{i}:30", Text = $"{i}:30" });
            }
        }
    }
}
