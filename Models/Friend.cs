using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibraryManagementSystem.Models
{

        public class Friend
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }


            public List<int> BooksCurrentlyBorrowed { get; set; } = new List<int>();



            public void BorrowBook(int bookId)
            {
                BooksCurrentlyBorrowed.Add(bookId);
            }



            public void ReturnBook(int bookId)
            {
                BooksCurrentlyBorrowed.Remove(bookId);
            }


        }
    

}
