using Microsoft.Extensions.Localization;
using Restaurant.Validators;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.ViewModels
{
    public class ReservationViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email.")]
        //[RegularExpression(@"^\w+([\.-]?\w+)*@\w+(-?\w+)*(\.[a-zA-Z]{2,3})+$", ErrorMessage = "Invalid Email.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        //[RegularExpression(@"^(\+\d{1,3}\s*?)?\d{10,20}$", ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Time")]
        public TimeSpan ReservationTime { get; set; } = DateTime.Now.TimeOfDay;

        [Required(ErrorMessage = "The {0} field is required.")]
        [Range(1, 10, ErrorMessage = "The number of guests allowed is: 1-10.")]
        [Display(Name = "Guests")]
        public int Guests { get; set; } = 1;

        [StringLength(1000)]
        [Display(Name = "Notes", Prompt = "Your notes (optional)")]
        public string? Note { get; set; }

        [MustBeTrue(ErrorMessage = "You must accept terms and conditions.")]
        public bool AcceptedTerms { get; set; }

        public string? Token { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var localizer = validationContext.GetService<IStringLocalizer<SharedResource>>();

            var ReservationDateTime = ReservationDate.Date.Add(ReservationTime);
            if (ReservationDateTime < DateTime.Now.AddHours(5))
                yield return new ValidationResult(localizer!["The reservation date and time is invalid."], new[] { nameof(ReservationDate), nameof(ReservationTime) });
        }
    }
}
