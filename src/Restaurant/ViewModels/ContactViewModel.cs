using Restaurant.Validators;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email.")]
        //[RegularExpression(@"^\w+([\.-]?\w+)*@\w+(-?\w+)*(\.[a-zA-Z]{2,3})+$", ErrorMessage = "Invalid Email.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        //[RegularExpression(@"^(\+\d{1,3}\s*?)?\d{10,20}$", ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(1000)]
        [Display(Name = "Message", Prompt = "Your message")]
        public string? Message { get; set; }

        [MustBeTrue(ErrorMessage = "You must accept terms and conditions.")]
        public bool AcceptedTerms { get; set; }
        public string? Token { get; set; }

    }
}
