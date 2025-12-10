using PersonalLibraryManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryAPI.DTOs
{
    public class BookStatusDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [EnumDataType(typeof(BookStatus))]
        public BookStatus Status { get; set; }
    }
}
