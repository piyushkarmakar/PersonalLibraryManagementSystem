using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalLibraryManagementSystem.Models;


namespace PersonalLibraryManagementSystem.Services
{



    public class LendingService
    {
        private List<LendingRecord> lendingRecords;
        private LibraryService libraryService;
        private FriendService friendService;
        private bool useDatabase = false;
        private DatabaseService dbService;


        // File Mode
        public LendingService(List<LendingRecord> records, LibraryService libraryService, FriendService friendService)
        {
            this.lendingRecords = records;
            this.libraryService = libraryService;
            this.friendService = friendService;
            this.useDatabase = false;
        }

        // SQL Mode
        public LendingService(string connectionString, LibraryService libraryService, FriendService friendService)
        {
            this.dbService = new DatabaseService(connectionString);
            this.libraryService = libraryService;
            this.friendService = friendService;
            this.lendingRecords = dbService.GetAllLendingRecords();
            this.useDatabase = true;
        }


        // Lend a book to a friend
        public bool LendBook(int bookId, string friendName, DateTime dueDate)
        {
            Book book = libraryService.GetById(bookId);
            Friend friend = friendService.GetByName(friendName);

            if (book == null || friend == null)
            {
                Console.WriteLine("⚠ Book or Friend not found.");
                return false;
            }

            if (book.IsLent)
            {
                Console.WriteLine("⚠ Book is already lent to someone else.");
                return false;
            }

            // Create a new lending record
            LendingRecord record = new LendingRecord();
            record.BookId = bookId;
            record.BookName = book.Title;
            record.FriendName = friend.Name;
            record.LendDate = DateTime.Now;
            record.DueDate = dueDate;
            record.ReturnDate = null;

            if (useDatabase)
            {
                dbService.AddLendingRecord(record);
                lendingRecords = dbService.GetAllLendingRecords();
            }
            else
            {
                lendingRecords.Add(record);
            }

            // Update friend & book status
            book.IsLent = true;
            friend.BorrowBook(bookId);

            Console.WriteLine("✅ Book lent successfully.");
            return true;
        
        }

        // Return a book
        public bool ReturnBook(int bookId, string friendName)
        {
            Book book = libraryService.GetById(bookId);
            Friend friend = friendService.GetByName(friendName);

            if (book == null || friend == null)
            {
                Console.WriteLine("⚠ Invalid book or friend name.");
                return false;
            }

            LendingRecord record = null;
            foreach (LendingRecord r in lendingRecords)
            {
                if (r.BookId == bookId &&
                    r.FriendName.Equals(friendName, StringComparison.OrdinalIgnoreCase) &&
                    !r.ReturnDate.HasValue)
                {
                    record = r;
                    break;
                }
            }

            if (record == null)
            {
                Console.WriteLine("⚠ No active lending record found for this book and friend.");
                return false;
            }

            if (useDatabase)
            {
                dbService.MarkAsReturned(bookId, friendName);
                lendingRecords = dbService.GetAllLendingRecords();
            }
            else
            {
                record.MarkAsReturned();
            }

            // Update status in memory
            book.IsLent = false;
            friend.ReturnBook(bookId);

            Console.WriteLine("✅ Book returned successfully.");
            return true;

        }



        // 🔹 Get all lending records
        public List<LendingRecord> GetAllRecords()
        {
            if (useDatabase)
            {
                return dbService.GetAllLendingRecords();
            }
            return lendingRecords;
        }

        // 🔹 Get all lent books
        public List<LendingRecord> GetAllLentBooks()
        {
            List<LendingRecord> lentBooks = new List<LendingRecord>();
            List<LendingRecord> source = useDatabase ? dbService.GetAllLendingRecords() : lendingRecords;

            foreach (LendingRecord record in source)
            {
                // only include books not yet returned
                if (!record.ReturnDate.HasValue)
                {
                    lentBooks.Add(record);
                }
            }

            return lentBooks;
        }



        // 🔹 Get overdue books
        public List<LendingRecord> GetOverdueBooks()
        {
            List<LendingRecord> overdueRecords = new List<LendingRecord>();
            List<LendingRecord> source = useDatabase ? dbService.GetAllLendingRecords() : lendingRecords;

            foreach (LendingRecord r in source)
            {
                if (!r.ReturnDate.HasValue && DateTime.Now > r.DueDate)
                {
                    overdueRecords.Add(r);
                }
            }

            return overdueRecords;
        }



    }

}