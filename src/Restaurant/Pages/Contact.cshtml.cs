using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Services.Email;
using Restaurant.Services.ReCaptcha;
using Restaurant.ViewModels;

namespace Restaurant.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IReCaptchaService _reCaptchaService;

        public ContactModel(IEmailService emailService,
            IConfiguration configuration, 
            IReCaptchaService reCaptchaService)
        {
            _emailService = emailService;
            _configuration = configuration;
            _reCaptchaService = reCaptchaService;
        }

        [BindProperty]
        public ContactViewModel Contact { get; set; } = new();
        public string? ReCaptchaSiteKey { get; set; }

        [TempData]
        public bool IsSuccessfullySent { get; set; }

        public void OnGet()
        {
            ReCaptchaSiteKey = _configuration["ReCaptcha:SiteKey"];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReCaptchaSiteKey = _configuration["ReCaptcha:SiteKey"];

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!await _reCaptchaService.IsValid(Contact.Token!, HttpContext.Connection.RemoteIpAddress!.ToString(), "contact"))
            {
                ModelState.AddModelError(string.Empty, "Invalid Captcha.");
                return Page();
            }

            //await _emailService.SendEmailAsync("myemail", "subject", "htmlContent");
            IsSuccessfullySent = true;

            return RedirectToPage();
        }

    }
}
