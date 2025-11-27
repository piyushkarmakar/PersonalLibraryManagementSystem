using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalLibraryManagementSystem.Enums;
using PersonalLibraryManagementSystem.Models;

namespace PersonalLibraryManagementSystem.Services
{

    public static class FileService
    {
        public static void SaveBooks(List<Book> books, string path)
        {
            List<string> lines = new List<string>();

            // Add header line
            lines.Add("===========================================================================================================================================");
            lines.Add("ID|BookName|Author|Genre|Status|Rating|DateAdded|DateStarted|DateFinished|Review|IsLent");
            lines.Add("===========================================================================================================================================");

            foreach (Book b in books)
            {
                string line = b.Id + "|" + b.Title + "|" + b.Author + "|" + b.Genre + "|" +
                              b.Status + "|" + b.Rating + "|" + b.DateAdded + "|" +
                              b.DateStarted + "|" + b.DateFinished + "|" + b.Review + "|" + b.IsLent;
                lines.Add(line);
            }
            File.WriteAllLines(path, lines);
        }

        public static List<Book> LoadBooks(string path)
        {
            List<Book> books = new List<Book>();
            if (!File.Exists(path)) return books;

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                // Skip header or empty lines
                if (string.IsNullOrWhiteSpace(line) ||
                    line.StartsWith("ID") ||
                    line.StartsWith("="))
                    continue;
                string[] parts = line.Split('|');
                if (parts.Length < 11)
                    continue;
                Book book = new()
                {
                    Id = int.Parse(parts[0]),
                    Title = parts[1],
                    Author = parts[2],
                    Genre = Enum.Parse<Genre>(parts[3]),
                    Status = Enum.Parse<BookStatus>(parts[4]),
                    Rating = int.Parse(parts[5]),
                    DateAdded = DateTime.Parse(parts[6]),
                    DateStarted = string.IsNullOrEmpty(parts[7]) ? null : DateTime.Parse(parts[7]),
                    DateFinished = string.IsNullOrEmpty(parts[8]) ? null : DateTime.Parse(parts[8]),
                    Review = parts[9],
                    IsLent = bool.Parse(parts[10])
                };
                books.Add(book);
            }
            return books;
        }

        public static List<Friend> LoadFriends(string path)
        {
            List<Friend> friends = new List<Friend>();
            if (!File.Exists(path)) return friends;

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                // Skip header or blank lines

                if (string.IsNullOrWhiteSpace(line) ||
                    line.StartsWith("Name") ||
                    line.StartsWith("="))
                    continue;
                string[] parts = line.Split('|');
                Friend friend = new()
                {
                    Name = parts[0],
                    Email = parts[1],
                    Phone = parts[2],

                    BooksCurrentlyBorrowed = new List<int>()
                };
                if (parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
                {
                    string[] borrowed = parts[3].Split(',');
                    foreach (string s in borrowed)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            friend.BooksCurrentlyBorrowed.Add(int.Parse(s));
                        }
                    }
                }

                friends.Add(friend);
            }
            return friends;
        }

        public static void SaveFriends(List<Friend> friends, string path)
        {
            List<string> lines = new List<string>();
            lines.Add("=====================================================");
            lines.Add("Name|Email|Phone");
            lines.Add("=====================================================");


            foreach (Friend f in friends)
            {
                string borrowed = string.Join(",", f.BooksCurrentlyBorrowed);
                string line = f.Name + "|" + f.Email + "|" + f.Phone + "|" + borrowed;
                lines.Add(line);
            }
            File.WriteAllLines(path, lines);
        }

        public static List<LendingRecord> LoadLendingRecords(string path)
        {
            List<LendingRecord> records = new List<LendingRecord>();
            if (!File.Exists(path)) return records;

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                // Skip header or empty line
                if (string.IsNullOrWhiteSpace(line) ||
                    line.StartsWith("ID") ||
                    line.StartsWith("="))
                    continue;
                string[] parts = line.Split('|');
                LendingRecord record = new()
                {
                    BookId = int.Parse(parts[0]),
                    BookName = parts[1],
                    FriendName = parts[2],
                    LendDate = DateTime.Parse(parts[3]),
                    DueDate = DateTime.Parse(parts[4]),
                    ReturnDate = string.IsNullOrEmpty(parts[5]) ? null : DateTime.Parse(parts[4])
                };
                records.Add(record);
            }
            return records;
        }

        public static void SaveLendingRecords(List<LendingRecord> lendingRecords, string path)
        {
            List<string> lines = new List<string>();
            lines.Add("==========================================================================================================");
            lines.Add("ID|BookName|FriendName|BorrowedOn|DueDate|Returned");
            lines.Add("==========================================================================================================");


            foreach (LendingRecord r in lendingRecords)
            {
                string line = r.BookId + "|" + r.BookName + "|" + r.FriendName + "|" + r.LendDate + "|" + r.DueDate + "|" + r.ReturnDate;
                lines.Add(line);
            }
            File.WriteAllLines(path, lines);
        }






    }
}