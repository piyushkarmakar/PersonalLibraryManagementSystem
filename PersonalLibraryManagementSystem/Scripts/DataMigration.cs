using System;
using System.Collections.Generic;
using PersonalLibraryManagementSystem.Models;
using PersonalLibraryManagementSystem.Services;

namespace PersonalLibraryManagementSystem.Scripts
{
    public static class DataMigration
    {
        public static void ImportFromFilesToDatabase()
        {
            // Set your file paths
            string bookFilePath = "C:\\ASP .NET\\LibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\books.txt";
            string friendFilePath = "C:\\ASP .NET\\LibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\friends.txt";
            string lendingFilePath = "C:\\ASP .NET\\LibraryManagementSystem\\PersonalLibraryManagementSystem\\Data\\lending_records.txt";

            // SQL connection
            string connectionString = "Data Source=localhost;Initial Catalog=PersonalLibraryDB;Integrated Security=True;TrustServerCertificate=True;";

            // Step 1: Load all from files
            List<Book> books = FileService.LoadBooks(bookFilePath);
            List<Friend> friends = FileService.LoadFriends(friendFilePath);
            List<LendingRecord> lendings = FileService.LoadLendingRecords(lendingFilePath);

            // Step 2: Initialize database service
            DatabaseService db = new DatabaseService(connectionString);

            // Step 3: Insert books
            Console.WriteLine("\n📚 Importing Books...");
            foreach (Book b in books)
            {
                db.AddBook(b);
            }

            // Step 4: Insert friends
            Console.WriteLine("\n👥 Importing Friends...");
            foreach (Friend f in friends)
            {
                db.AddFriend(f);
            }

            // Step 5: Insert lending records
            Console.WriteLine("\n📖 Importing Lending Records...");
            foreach (LendingRecord l in lendings)
            {
                db.AddLendingRecord(l);
            }

            Console.WriteLine("\n✅ Migration Completed Successfully!");
        }
    }
}

