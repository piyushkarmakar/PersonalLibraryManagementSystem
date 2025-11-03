using System;
using PersonalLibraryManagementSystem.Services;
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
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Book ID: ");
                        libraryService.StartReading(int.Parse(Console.ReadLine() ?? "0"));
                        Console.WriteLine("Book marked as Currently Reading!");
                        break;

                    case "2":
                        Console.Write("Enter Book ID: ");
                        libraryService.FinishReading(int.Parse(Console.ReadLine() ?? "0"));
                        Console.WriteLine("Book marked as Finished!");
                        break;

                    case "3":
                        Console.Write("Enter Book ID: ");
                        int rateId = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter rating (1-5): ");
                        int rating = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter review: ");
                        string review = Console.ReadLine();
                        libraryService.AddRating(rateId, rating, review);
                        Console.WriteLine("Rating & Review added!");
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
    }
}
