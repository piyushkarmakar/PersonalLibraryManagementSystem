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
                            Console.WriteLine("Friend added successfully!");
                            break;
                        }
                    case "2":

                        Console.WriteLine("\n--- Friends List ---");

                        List<Friend> friends = (List<Friend>)friendService.GetAllFriends();
                        foreach (Friend friend in friends)
                        {
                            Console.WriteLine(friend.Name + " | " + friend.Email + " | " + friend.Phone);
                        }
                        break;

                    case "3":
                        Console.Write("Book ID: ");
                        int bookId = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Friend Name: ");
                        string friendName = Console.ReadLine();

                        Console.Write("Due Date (yyyy-mm-dd): ");
                        DateTime dueDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

                        lendingService.LendBook(bookId, friendName, dueDate);
                        Console.WriteLine("Book lent successfully!");
                        break;

                    case "4":
                        Console.Write("Enter Book ID to return: ");
                        int returnBookId = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Enter Friend Name: ");
                        string returnFriend = Console.ReadLine();

                        lendingService.ReturnBook(returnBookId, returnFriend);
                        Console.WriteLine("Book returned successfully!");
                        break;

                    case "5":
                        Console.WriteLine("\n--- Lending Records ---");

                        foreach (var record in lendingService.GetAllRecords())
                        {
                            string returnDate = record.ReturnDate?.ToString("yyyy-MM-dd") ?? "Pending";
                            Console.WriteLine($"{record.BookId} | {record.FriendName} | {record.LendDate:yyyy-MM-dd} -> {record.DueDate:yyyy-MM-dd} | Returned: {returnDate}");
                        }

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
