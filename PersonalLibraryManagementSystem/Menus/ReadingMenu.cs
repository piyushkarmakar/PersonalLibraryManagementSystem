using System;
using PersonalLibraryManagementSystem.Services;

namespace PersonalLibraryManagementSystem.Menus
{
    public static class ReadingMenu
    {
        public static void Show(LibraryService libraryService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("          READING PROGRESS");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Start Reading a Book");
                Console.WriteLine("2. Finish Reading a Book");
                Console.WriteLine("3. Add Rating & Review");
                Console.WriteLine("4. View Rating and Review of Book");
                Console.WriteLine("5. View Reading Progress of All Books");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                   START READING A BOOK         "    );
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Book id : Book Name");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id}:{book.Title}\n");
                        Console.Write("Enter Book ID: ");
                        libraryService.StartReading(int.Parse(Console.ReadLine() ?? "0"));
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Book status updated to Currently Reading!");
                        break;

                    case "2":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                   FINISH READING A BOOK         ");
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Book id : Book Name");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id}:{book.Title}\n");
                        Console.Write("Enter Book ID: ");
                        libraryService.FinishReading(int.Parse(Console.ReadLine() ?? "0"));
                        Console.WriteLine("Book marked as Finished!");
                        Console.WriteLine("=========================================================================================");

                        break;

                    case "3":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       ADD RATING AND REVIEW");
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Book id : Book Name");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id}:{book.Title}\n");
                        Console.Write("Enter Book ID: ");
                        int rateId = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter rating (1-5): ");
                        int rating = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter review: ");
                        string review = Console.ReadLine();
                        libraryService.AddRating(rateId, rating, review);
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Rating & Review added!");
                        break;
                    case "4":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       VIEW RATINGS & REVIEW ");
                        Console.WriteLine("=========================================================================================");
                        foreach (var book in libraryService.GetAllBooks())
                        {
                            Console.Write($"{book.Id}:{book.Title}\n");
                            Console.WriteLine($"Rating: {(book.Rating > 0 ? book.Rating.ToString() : "Not Rated")}");
                            Console.WriteLine($"Review: {(string.IsNullOrEmpty(book.Review) ? "No Review Added" : book.Review)}");
                            Console.WriteLine("=========================================================================================");
                        }
                        break;
                    case "5":
                        Console.WriteLine("================================================================================================================");
                        Console.WriteLine("                       VIEW BOOKSTATUS ");
                        Console.WriteLine("================================================================================================================");
                        ViewAllBooksProgress(libraryService);
                        Console.WriteLine("================================================================================================================");
                        
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        private static void ViewAllBooksProgress(LibraryService libraryService)
        {
            var books = libraryService.GetAllBooks();

            Console.WriteLine("================================================================================================================");
            Console.WriteLine("{0,-5} {1,-40} {2,-15}",
                "ID", "Title", "Status");
            Console.WriteLine("================================================================================================================");

            foreach (var book in books)
            {
                string ratingText = book.Rating > 0 ? book.Rating.ToString() : "Not Rated";
                string reviewText = string.IsNullOrEmpty(book.Review) ? "No Review Added" : book.Review;

                Console.WriteLine("{0,-5} {1,-40} {2,-15}",
                    book.Id,
                    book.Title.Length > 40 ? book.Title.Substring(0, 37) + "..." : book.Title,
                    book.Status);
            }

            Console.WriteLine("================================================================================================================");
        }

    }
}
