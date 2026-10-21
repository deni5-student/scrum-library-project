namespace SchoolLibrary.Services
{
    public class LoanResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public DateTime? DueDate { get; set; }
    }
}