using System;
using System.Reflection;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;
namespace PersonalLibraryManagementSystem.Menus
{
    public static class FriendsLendingMenu
    {
        private static void PrintLendingRecords(List<LendingRecord> records, LibraryService libraryService)
        {
            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-15} {5,-15}",
                "ID", "Book Name", "Friend Name", "Lend Date", "Due Date", "Return Date");
            Console.WriteLine("================================================================================");

            foreach (LendingRecord record in records)
            {
                Book book = libraryService.GetById(record.BookId);
                string bookTitle = !string.IsNullOrEmpty(record.BookName)
                    ? record.BookName
                    : (book != null ? book.Title : "Unknown");
                string returnDate = record.ReturnDate.HasValue ? record.ReturnDate.Value.ToString("yyyy-MM-dd") : "Pending";

                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-15} {5,-15}",
                    record.BookId,
                    bookTitle,
                    record.FriendName,
                    record.LendDate.ToString("yyyy-MM-dd"),
                    record.DueDate.ToString("yyyy-MM-dd"),
                    returnDate);
            }
            Console.WriteLine("================================================================================");
        }
        public static void Show(FriendService friendService, LendingService lendingService, LibraryService libraryService)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("          FRIENDS & LENDING");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Add Friend");
                Console.WriteLine("2. View All Friends");
                Console.WriteLine("3. Lend a Book");
                Console.WriteLine("4. Return a Book");
                Console.WriteLine("5. View Lending Records");
                Console.WriteLine("6. View Currently Lent Books");
                Console.WriteLine("7. View Overdue Books");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        {
                            Console.Write("Enter Friend Name: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine();

                            Console.Write("Enter Phone: ");
                            string phone = Console.ReadLine();

                            Friend friend = new Friend();
                            friend.Name = name;
                            friend.Email = email;
                            friend.Phone = phone;

                            friendService.Add(friend);
                            Console.WriteLine("=========================================================================================");

                            Console.WriteLine("\n Friend added successfully!");

                            break;
                        }
                    case "2":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                FRIENDS LIST                           ");
                        Console.WriteLine("=========================================================================================");

                        List<Friend> friends = (List<Friend>)friendService.GetAllFriends();
                        foreach (Friend friend in friends)
                        {
                            Console.WriteLine(friend.Name + " | " + friend.Email + " | " + friend.Phone);
                        }
                        Console.WriteLine("=========================================================================================");

                        break;

                    case "3":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       LEND A BOOK");
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("\n============================================================");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id} : {book.Title}\n");
                        Console.WriteLine("==============================================================");

                        Console.Write("Book ID: ");
                        int bookId = int.Parse(Console.ReadLine() ?? "0");



                        Console.Write("Friend Name: ");
                        string friendName = Console.ReadLine();

                        Console.Write("Due Date (yyyy-mm-dd): ");
                        DateTime dueDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

                        lendingService.LendBook(bookId, friendName, dueDate);
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("\n Book lent successfully!");
                        break;

                    case "4":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                       RETURN BOOK");
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("\n==============================================================");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id} : {book.Title}\n");
                        Console.WriteLine("================================================================");

                        Console.Write("Enter Book ID to return: ");
                        int returnBookId = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Enter Friend Name: ");
                        string returnFriend = Console.ReadLine();

                        lendingService.ReturnBook(returnBookId, returnFriend);
                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("\n Book returned successfully!");
                        break;

                    case "5":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                  LENDING RECORDS");
                        Console.WriteLine("=========================================================================================");
                        PrintLendingRecords(lendingService.GetAllRecords(), libraryService);
                        Console.WriteLine("=========================================================================================");

                        break;

                    case "6":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("               CURRENTLY LENT BOOKS");
                        Console.WriteLine("=========================================================================================");
                        PrintLendingRecords(lendingService.GetAllLentBooks(), libraryService);
                        Console.WriteLine("=========================================================================================");

                        break;

                    case "7":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                 OVERDUE BOOKS");
                        Console.WriteLine("=========================================================================================");
                        List<LendingRecord> overdue = lendingService.GetOverdueBooks();
                        if (overdue == null || overdue.Count == 0)
                        {
                            Console.WriteLine("No overdue books found.");
                        }
                        else
                        {
                            PrintLendingRecords(overdue, libraryService);
                        }
                        Console.WriteLine("=========================================================================================");

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