using System;
using PersonalLibraryManagementSystem.Enums;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;
namespace PersonalLibraryManagementSystem.Menus
{
    public static class BookMenu
    {
        public static void Show(LibraryService libraryService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("          BOOK MANAGEMENT");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Add New Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Search Book");
                Console.WriteLine("4. Update Book");
                Console.WriteLine("5. Remove Book");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       ADD NEW A BOOK IN LIBRARY");
                        Console.WriteLine("=========================================================================================");

                        Console.Write("\n\n Enter title: ");
                        string title = Console.ReadLine();

                        Console.Write("Enter author: ");
                        string author = Console.ReadLine();



                        // Print all the gernre values present to select
                        Console.WriteLine("\nSelect genre:");

                        Array genres = Enum.GetValues<Genre>();
                        for (int i = 0; i < genres.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
                        }

                        //Select a choice among the genre choices given above
                        Console.Write("Your choice: ");
                        string input = Console.ReadLine();
                        int genreChoice = 1;

                        if (!string.IsNullOrEmpty(input))
                        {
                            genreChoice = int.Parse(input);
                        }

                        Genre genre = (Genre)(genreChoice - 1);


                        Book newBook = new Book();
                        newBook.Id = libraryService.GenerateNextBookId();
                        newBook.Title = title;
                        newBook.Author = author;
                        newBook.Genre = genre;
                        newBook.Status = BookStatus.ToRead;
                        newBook.DateAdded = DateTime.Now;
                        newBook.DateStarted = null;         // Not started yet
                        newBook.DateFinished = null;        // Not finished yet
                        newBook.Review = "";                // Empty review
                        newBook.IsLent = false;             // Not lent out yet

                        libraryService.Add(newBook);
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("\nBook \"" + title + "\" added successfully!");
                        break;

                    case "2":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       VIEW ALL BOOKS");
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("=========================================================================================");
                        
                        foreach (var book in libraryService.GetAllBooks())
                            book.DisplayDetails();
                        Console.WriteLine("=========================================================================================");

                        break;

                    case "3":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       SEARCH A BOOK");
                        Console.WriteLine("=========================================================================================");

                        Console.Write("\nEnter title or author to search: ");
                        string query = Console.ReadLine();
                        foreach (var book in libraryService.Search(query))
                            book.DisplayDetails();
                        Console.WriteLine("=========================================================================================");
                        break;

                    case "4":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       UPDATE A BOOK ");
                        Console.WriteLine("=========================================================================================");

                        Console.Write("Enter Book ID to update: \n");
                        Console.WriteLine("Book id : Book Name");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id}:{book.Title}\n");
                            
                        int updateId = int.Parse(Console.ReadLine() ?? "0");
                        var bookToUpdate = libraryService.GetById(updateId);
                        if (bookToUpdate != null)
                        {
                            Console.Write("New Title (leave blank to keep same): ");
                            string newTitle = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newTitle))
                                bookToUpdate.Title = newTitle;

                            Console.Write("New Author (leave blank to keep same): ");
                            string newAuthor = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newAuthor))
                                bookToUpdate.Author = newAuthor;

                            Console.WriteLine("Select new status: 1. ToRead  2. CurrentlyReading  3. Finished");
                            int newStatus = int.Parse(Console.ReadLine() ?? "1");
                            bookToUpdate.Status = (BookStatus)(newStatus - 1);


                            Console.Write("Enter new rating (1-5, or 0 to skip): ");
                            string ratingInput = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(ratingInput))
                            {
                                int newRating = int.Parse(ratingInput);
                                if (newRating >= 1 && newRating <= 5)
                                    bookToUpdate.Rating = newRating;
                            }

                            Console.Write("Enter new review (leave blank to keep same): ");
                            string newReview = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newReview))
                                bookToUpdate.Review = newReview;

                            libraryService.Update(updateId, bookToUpdate);
                            Console.WriteLine("=========================================================================================");

                            Console.WriteLine("\nBook updated successfully!");
                        }
                        else Console.WriteLine("Book not found.");
                        break;

                    case "5":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       REMOVE A BOOK");
                        Console.WriteLine("=========================================================================================");

                        Console.Write("Enter Book ID to remove: \n");
                        Console.WriteLine("Book id : Book Name");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id}:{book.Title}\n");
                        int removeId = int.Parse(Console.ReadLine() ?? "0");
                        libraryService.Remove(removeId);
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Book removed successfully!");
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
