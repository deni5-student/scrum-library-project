using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Brugernavn er påkrævet")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Brugernavn skal være mellem 3 og 50 tegn")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Password er påkrævet")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password skal være mindst 6 tegn")]
        public string Password { get; set; } = "";
    }
}