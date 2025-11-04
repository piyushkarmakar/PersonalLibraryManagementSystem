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

        public LendingService(List<LendingRecord> lendingRecords, LibraryService library, FriendService friends)
        {
            this.lendingRecords = lendingRecords;
            this.libraryService = library;
            this.friendService = friends;
        }

        // Lend a book to a friend
        public bool LendBook(int bookId, string friendName, DateTime dueDate)
        {
            var book = libraryService.GetById(bookId);
            var friend = friendService.Search(friendName).FirstOrDefault();

            if (book == null || friend == null || book.IsLent)
                return false;

            // Mark book as lent
            book.IsLent = true;

            // Add book to friend's borrowed list
            friend.BorrowBook(bookId);

            // Create a lending record
            lendingRecords.Add(new LendingRecord
            {
                BookId = bookId,
                FriendName = friend.Name,
                LendDate = DateTime.Now,
                DueDate = dueDate
            });

            return true;
        }

        // Return a book
        public bool ReturnBook(int bookId, string friendName)
        {
            LendingRecord record = null;
            foreach (var r in lendingRecords)
            {
                if (r.BookId == bookId &&
                    r.FriendName != null &&
                    r.FriendName.Equals(friendName, StringComparison.OrdinalIgnoreCase) &&
                    r.ReturnDate == null)
                {
                    record = r;
                    break;
                }
            }

            Book book = libraryService.GetById(bookId);
            Friend friend = null;
            foreach (var f in friendService.Search(friendName))
            {
                friend = f;
                break;
            }

            if (record == null || book == null || friend == null)
            {
                return false;
            }

            // Mark as returned
            record.MarkAsReturned();
            book.IsLent = false;
            friend.ReturnBook(bookId);

            return true;
        }



        public List<LendingRecord> GetAllRecords()
        {
            return lendingRecords;
        }




        // View all lent books
        public List<LendingRecord> GetAllLentBooks()
        {
            return lendingRecords;
        }


        // Check overdue books
        public List<LendingRecord> GetOverdueBooks()
        {
            List<LendingRecord> overdueRecords = new List<LendingRecord>();

            foreach (LendingRecord r in lendingRecords)
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