using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PersonalLibraryManagementSystem.Models
{

        public class Friend
        {

            public int Id { get; set; }

            [Required(ErrorMessage = "Friend name is required")]
            [MaxLength(60)]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required]
            [Phone(ErrorMessage = "Invalid phone number")]
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
