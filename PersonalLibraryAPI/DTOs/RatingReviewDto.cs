using System.ComponentModel.DataAnnotations;

namespace PersonalLibraryAPI.DTOs
{
    public class RatingReviewDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Review cannot exceed 500 characters")]
        public string Review { get; set; }
    }
}
