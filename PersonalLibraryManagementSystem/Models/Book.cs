using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalLibraryManagementSystem.Enums;
using System.Text.Json.Serialization;



namespace PersonalLibraryManagementSystem.Models
{
    public class Book : LibraryItem
    {
        [JsonPropertyOrder(4)]
        public string Author { get; set; }
        [JsonPropertyOrder(5)]

        public Genre Genre { get; set; }
        [JsonPropertyOrder(6)]

        public BookStatus Status { get; set; }
        [JsonPropertyOrder(7)]

        public int Rating { get; set; } // 0-5
        [JsonPropertyOrder(8)]

        public DateTime? DateStarted { get; set; }
        [JsonPropertyOrder(9)]

        public DateTime? DateFinished { get; set; }
        [JsonPropertyOrder(10)]

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
