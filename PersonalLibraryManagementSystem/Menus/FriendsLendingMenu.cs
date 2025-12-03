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
            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-15} {5,-15} {6,-20}",
                "ID", "Book Name", "Friend Name", "Lend Date", "Due Date", "Return Date", "Overdue days");
            Console.WriteLine("========================================================================================================================");

            foreach (LendingRecord record in records)
            {
                Book book = libraryService.GetById(record.BookId);
                string bookTitle = !string.IsNullOrEmpty(record.BookName)
                    ? record.BookName
                    : (book != null ? book.Title : "Unknown");
                string returnDate = record.ReturnDate.HasValue ? record.ReturnDate.Value.ToString("yyyy-MM-dd") : "Pending";
                string overdueInfo = "";
                if (!record.ReturnDate.HasValue && DateTime.Now > record.DueDate)
                {
                    overdueInfo = $" Overdue by {(DateTime.Now - record.DueDate).Days} days";
                }
                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-15} {5,-15} {6,-20}",
                    record.BookId,
                    bookTitle,
                    record.FriendName,
                    record.LendDate.ToString("yyyy-MM-dd"),
                    record.DueDate.ToString("yyyy-MM-dd"),
                    returnDate, overdueInfo);
            }
            Console.WriteLine("========================================================================================================================");
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
                Console.WriteLine("3. Update Friend");
                Console.WriteLine("4. Delete Friend");
                Console.WriteLine("5. Lend a Book");
                Console.WriteLine("6. Return a Book");
                Console.WriteLine("7. View Lending Records");
                Console.WriteLine("8. View Currently Lent Books");
                Console.WriteLine("9. View Overdue Books");
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
                            Console.WriteLine("======================================================================");

                            Console.WriteLine("\n Friend added successfully!");

                            break;
                        }
                    case "2":
                        var friends = friendService.GetAllFriends().ToList();

                        if (friends.Count == 0)
                        {
                            Console.WriteLine("No friends found.");
                        }
                        else
                        {
                            Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15}", "ID", "Name", "Email", "Phone");
                            Console.WriteLine("--------------------------------------------------------------------");

                            foreach (var friend in friends)
                            {
                                Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15}",
                                    friend.Id, friend.Name, friend.Email, friend.Phone);
                            }
                        }

                        Console.WriteLine("======================================================================");
                        break;
                    case "3":
                        Console.WriteLine("======================================================================");
                        Console.WriteLine("                     UPDATE FRIEND");
                        Console.WriteLine("======================================================================");
                        var friendsForUpdate = friendService.GetAllFriends().ToList();
                        if (friendsForUpdate.Count == 0)
                        {
                            Console.WriteLine("No friends available to update.");
                            break;
                        }

                        Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15}", "ID", "Name", "Email", "Phone");
                        Console.WriteLine("--------------------------------------------------------------------");

                        foreach (var fr in friendsForUpdate)
                        {
                            Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15}",
                                fr.Id, fr.Name, fr.Email, fr.Phone);
                        }

                        Console.Write("\nEnter Friend ID to update: ");
                        int updateId = int.Parse(Console.ReadLine());

                        // ✔ FIXED – Find friend by actual Friend.Id, not list index
                        Friend existingFriend = friendsForUpdate.FirstOrDefault(f => f.Id == updateId);

                        if (existingFriend == null)
                        {
                            Console.WriteLine("Friend not found!");
                            break;
                        }

                        Console.Write("New Name (Enter to skip): ");
                        string nn = Console.ReadLine();
                        Console.Write("New Email (Enter to skip): ");
                        string ne = Console.ReadLine();
                        Console.Write("New Phone (Enter to skip): ");
                        string np = Console.ReadLine();

                        Friend updatedFriend = new Friend
                        {
                            Id = existingFriend.Id,
                            Name = string.IsNullOrEmpty(nn) ? existingFriend.Name : nn,
                            Email = string.IsNullOrEmpty(ne) ? existingFriend.Email : ne,
                            Phone = string.IsNullOrEmpty(np) ? existingFriend.Phone : np
                        };

                        friendService.Update(updateId, updatedFriend);

                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Friend updated successfully!");
                        break;
                    case "4":
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                     DELETE FRIEND");
                        Console.WriteLine("=========================================================================================");

                        var friendsForDelete = friendService.GetAllFriends().ToList();

                        if (friendsForDelete.Count == 0)
                        {
                            Console.WriteLine("No friends available to delete.");
                            break;
                        }

                        Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15}", "ID", "Name", "Email", "Phone");
                        Console.WriteLine("--------------------------------------------------------------------");

                        foreach (var fr in friendsForDelete)
                        {
                            Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15}",
                                fr.Id, fr.Name, fr.Email, fr.Phone);
                        }

                        Console.Write("\nEnter Friend ID to delete: ");
                        int deleteId = int.Parse(Console.ReadLine());

                        // ✔ FIXED — find by real Id
                        Friend friendToDelete = friendsForDelete.FirstOrDefault(f => f.Id == deleteId);

                        if (friendToDelete == null)
                        {
                            Console.WriteLine("Friend not found!");
                            break;
                        }

                        friendService.Remove(deleteId);

                        Console.WriteLine("=========================================================================================");

                        Console.WriteLine("Friend deleted successfully!");
                        break;


                    case "5":
                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("                       LEND A BOOK");
                        Console.WriteLine("========================================================================================================================");

                        Console.WriteLine("========================================================================================================================");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id} : {book.Title}\n");
                        Console.WriteLine("========================================================================================================================");

                        Console.Write("Book ID: ");
                        int bookId = int.Parse(Console.ReadLine() ?? "0");



                        Console.Write("Friend Name: ");
                        string friendName = Console.ReadLine();

                        Console.Write("Due Date (yyyy-mm-dd): ");
                        DateTime dueDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString("yyyy-MM-dd"));

                        lendingService.LendBook(bookId, friendName, dueDate);
                        Console.WriteLine("========================================================================================================================");

                        Console.WriteLine("\n Book lent successfully!");
                        break;

                    case "6":
                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("                       RETURN BOOK");
                        Console.WriteLine("========================================================================================================================");

                        Console.WriteLine("\n======================================================================================================================");
                        foreach (var book in libraryService.GetAllBooks())
                            Console.Write($"{book.Id} : {book.Title}\n");
                        Console.WriteLine("========================================================================================================================");

                        Console.Write("Enter Book ID to return: ");
                        int returnBookId = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Enter Friend Name: ");
                        string returnFriend = Console.ReadLine();

                        lendingService.ReturnBook(returnBookId, returnFriend);
                        Console.WriteLine("========================================================================================================================");

                        Console.WriteLine("\n Book returned successfully!");
                        break;

                    case "7":
                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("                  LENDING RECORDS");
                        Console.WriteLine("========================================================================================================================");
                        PrintLendingRecords(lendingService.GetAllRecords(), libraryService);
                        Console.WriteLine("========================================================================================================================");

                        break;

                    case "8":
                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("               CURRENTLY LENT BOOKS");
                        Console.WriteLine("========================================================================================================================");
                        PrintLendingRecords(lendingService.GetAllLentBooks(), libraryService);
                        Console.WriteLine("========================================================================================================================");

                        break;

                    case "9":
                        Console.WriteLine("========================================================================================================================");
                        Console.WriteLine("                 OVERDUE BOOKS");
                        Console.WriteLine("========================================================================================================================");
                        List<LendingRecord> overdue = lendingService.GetOverdueBooks();
                        if (overdue == null || overdue.Count == 0)
                        {
                            Console.WriteLine("No overdue books found.");
                        }
                        else
                        {
                            PrintLendingRecords(overdue, libraryService);
                        }
                        Console.WriteLine("========================================================================================================================");

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