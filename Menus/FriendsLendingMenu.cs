using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;
using System;
using System.Reflection;
namespace PersonalLibraryManagementSystem.Menus
{
    public static class FriendsLendingMenu
    {
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
                        Console.WriteLine("==============================================================");
                        Console.WriteLine("                FRIENDS LIST                           ");
                        Console.WriteLine("==============================================================");

                        List<Friend> friends = (List<Friend>)friendService.GetAllFriends();
                        foreach (Friend friend in friends)
                        {
                            Console.WriteLine(friend.Name + " | " + friend.Email + " | " + friend.Phone);
                        }
                        Console.WriteLine("==============================================================");

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
                        Console.WriteLine("==============================================================");

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
                        Console.WriteLine("                       LENDING RECORDS");
                        Console.WriteLine("=========================================================================================");

                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id} : {book.Title} \n");

                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-12} {4,-12} {5,-15}",
                            "ID", "Book Name", "Friend Name", "Borrowed On", "Due Date", "Returned");
                        Console.WriteLine("=========================================================================================");



                        foreach (var record in lendingService.GetAllRecords())
                        {
                            var book = libraryService.GetById(record.BookId);
                            string bookTitle = book != null ? book.Title : "Unknown";
                            string returned = record.ReturnDate.HasValue
                                ? record.ReturnDate.Value.ToString("yyyy-MM-dd")
                                : "Pending";

                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-12} {4,-12} {5,-15}",
                                record.BookId,
                                bookTitle,
                                record.FriendName,
                                record.LendDate.ToString("yyyy-MM-dd"),
                                record.DueDate.ToString("yyyy-MM-dd"),
                                returned);
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