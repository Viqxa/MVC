using System.ComponentModel.DataAnnotations;

namespace RecipesApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(100)]
        public string Surname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

    }
}