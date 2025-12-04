using PersonalLibraryManagementSystem.Enums;

namespace PersonalLibraryAPI.DTOs
{
    public class BookStatusDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public BookStatus Status { get; set; }
    }
}
