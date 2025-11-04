using System;
using System.Collections.Generic;
using System.IO;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;
using PersonalLibraryManagementSystem.Enums;
using PersonalLibraryManagementSystem.Menus;


namespace PersonalLibraryManagementSystem
{
    class Program
    {
        static void Main(String[] arges)
        {

            string bookFilePath = "C:\\ASP .NET\\LibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\books.txt";
            string friendFilePath = "C:\\ASP .NET\\LibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\friends.txt";
            string lendingFilePath = "C:\\ASP .NET\\LibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\lending_records.txt";


            List<Book> books = FileService.LoadBooks(bookFilePath);
            List<Friend> friends = FileService.LoadFriends(friendFilePath);
            List<LendingRecord> lendings = FileService.LoadLendingRecords(lendingFilePath);

            LibraryService libraryService = new LibraryService(books);
            FriendService friendService = new FriendService(friends);
            LendingService lendingService = new LendingService(lendings, libraryService, friendService);

            // var books = FileService.LoadBooks(bookFilePath);
            // var friends = FileService.LoadFriends(friendFilePath);
            // var lendings = FileService.LoadLendingRecords(lendingFilePath);

            // var libraryService = new LibraryService(books);
            // var friendService = new FriendService(friends);
            // var lendingService = new LendingService(lendings, libraryService, friendService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("     PERSONAL LIBRARY MANAGEMENT SYSTEM");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Book Management");
                Console.WriteLine("2. Reading Progress");
                Console.WriteLine("3. Friends & Lending");
                Console.WriteLine("0. Exit");
                Console.Write("\nSelect option: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1":
                        BookMenu.Show(libraryService);
                        break;
                    case "2":
                        ReadingMenu.Show(libraryService);
                        break;
                    case "3":
                        FriendsLendingMenu.Show(friendService, lendingService, libraryService);
                        break;
                    case "0":
                        FileService.SaveBooks(books, bookFilePath);
                        FileService.SaveFriends(friends, friendFilePath);
                        FileService.SaveLendingRecords(lendings, lendingFilePath);
                        Console.WriteLine("\nData saved successfully. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }


        }

    }
}