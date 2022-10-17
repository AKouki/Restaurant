using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
    }
}
