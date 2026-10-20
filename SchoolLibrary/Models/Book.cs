namespace SchoolLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public int Total { get; set; }
        public int Available { get; set; }
    }
}