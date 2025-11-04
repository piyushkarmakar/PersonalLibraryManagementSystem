using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalLibraryManagementSystem.Enums;


namespace PersonalLibraryManagementSystem.Models
{
    public class Book : LibraryItem
    {
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public BookStatus Status { get; set; }
        public int Rating { get; set; } // 0-5
        public DateTime? DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }
        public string Review { get; set; }
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
