using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Models
{
    public class BookModel
    {
        [Required(ErrorMessage = "Titel er påkrævet")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Titel må højst være 100 tegn")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Forfatter er påkrævet")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Forfatter må højst være 100 tegn")]
        public string Author { get; set; } = "";

        [Range(1, 100, ErrorMessage = "Antal eksemplarer skal være mellem 1 og 100")]
        public int Total { get; set; } = 1;
    }
}