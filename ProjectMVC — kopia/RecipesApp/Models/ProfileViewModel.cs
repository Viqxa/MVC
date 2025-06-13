using System.ComponentModel.DataAnnotations;

namespace RecipesApp.Models
{
    public class ProfileViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; } = "";

        [Required, Display(Name = "First Name")]
        public string Name { get; set; } = "";

        [Required, Display(Name = "Surname")]
        public string Surname { get; set; } = "";

        [DataType(DataType.Password), Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
