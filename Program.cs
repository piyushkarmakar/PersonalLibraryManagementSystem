using PersonalLibraryManagementSystem.Enums;
using PersonalLibraryManagementSystem.Menus;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Scripts;
using PersonalLibraryManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.IO;


namespace PersonalLibraryManagementSystem
{
    class Program
    {
        static void Main(String[] arges)
        {

            // STEP 1: Define your SQL Server connection string
            string connectionString = "Data Source=localhost;Initial Catalog=PersonalLibraryDB;Integrated Security=True;TrustServerCertificate=True;";


            string bookFilePath = "C:\\ASP .NET\\PersonalLibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\books.txt";
            string friendFilePath = "C:\\ASP .NET\\PersonalLibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\friends.txt";
            string lendingFilePath = "C:\\ASP .NET\\PersonalLibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\lending_records.txt";

            // Ask user for mode
            Console.WriteLine("========================================");
            Console.WriteLine("   Choose Storage Mode");
            Console.WriteLine("========================================");
            Console.WriteLine("1. SQL Database");
            Console.WriteLine("2. Local Files");
            Console.WriteLine("0. Exit");

            Console.Write("Select option: ");
            string choice = Console.ReadLine();
            if (choice == "0")
            {
                Console.WriteLine("\n Goodbye!");
                return; // Exit the program immediately
            }

            bool useDatabase = (choice == "1");

            // Service initialization
            LibraryService libraryService;
            FriendService friendService;
            LendingService lendingService;


            if (useDatabase)
            {
                Console.WriteLine("\n Connected to SQL Database mode.\n");
                libraryService = new LibraryService(connectionString);
                friendService = new FriendService(connectionString);
                lendingService = new LendingService(connectionString, libraryService, friendService);
            }
            else
            {
                Console.WriteLine("\n Using Local File mode.\n");

                List<Book> books = FileService.LoadBooks(bookFilePath);
                List<Friend> friends = FileService.LoadFriends(friendFilePath);
                List<LendingRecord> lendings = FileService.LoadLendingRecords(lendingFilePath);

                libraryService = new LibraryService(books);
                friendService = new FriendService(friends);
                lendingService = new LendingService(lendings, libraryService, friendService);
            }
            //Very important to migrate data only once from files to database
            // 🔹 Temporary one-time data migration
            //DataMigration.ImportFromFilesToDatabase();
            //return;



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
                        // Only save to file if in file mode
                        if (!useDatabase)
                        {
                            Console.WriteLine("\n Saving data to files...");
                            FileService.SaveBooks(libraryService.GetAllBooks(), bookFilePath);
                            FileService.SaveFriends(friendService.GetAllFriends(), friendFilePath);
                            FileService.SaveLendingRecords(lendingService.GetAllRecords(), lendingFilePath);
                            Console.WriteLine("\n Data saved to files successfully!");
                        }
                        Console.WriteLine("\nGoodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }


        }

    }
}