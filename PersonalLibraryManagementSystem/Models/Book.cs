using PersonalLibraryManagementSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



namespace PersonalLibraryManagementSystem.Models
{
    public class Book : LibraryItem
    {
        [JsonPropertyOrder(4)]
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [JsonPropertyOrder(5)]
        [Required]

        public Genre Genre { get; set; }

        [JsonPropertyOrder(6)]
        [Required]

        public BookStatus Status { get; set; }

        [JsonPropertyOrder(7)]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]

        public int Rating { get; set; } // 0-5
        [JsonPropertyOrder(8)]

        public DateTime? DateStarted { get; set; }
        [JsonPropertyOrder(9)]

        public DateTime? DateFinished { get; set; }

        [JsonPropertyOrder(10)]
        [StringLength(500, ErrorMessage = "Review cannot exceed 500 characters")]

        public string Review { get; set; }
        [JsonPropertyOrder(11)]

        public bool IsLent { get; set; }



        public void MarkAsReading()
        {
            Status = BookStatus.CurrentlyReading;
            DateStarted = DateTime.Now;
        }

        public void FinishReading(int rating, string review)
        {
            Status = BookStatus.Finished;
            Rating = rating;
            Review = review;
            DateFinished = DateTime.Now;
        }

        public override void DisplayDetails()
        {
            Console.WriteLine($"{Id}: {Title} by {Author} | Genre: {Genre} | Status: {Status}");
            Console.WriteLine($"Rating: {(Rating > 0 ? Rating.ToString() : "Not Rated")}");
            Console.WriteLine($"Review: {(string.IsNullOrWhiteSpace(Review) ? "No Review" : Review)} \n");
        }

    }
}
