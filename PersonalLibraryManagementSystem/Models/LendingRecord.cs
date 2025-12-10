using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibraryManagementSystem.Models
{
    public class LendingRecord
    {


        public int BookId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Friend name is required")]
        [MaxLength(60)]
        public string FriendName { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Add OverdueDays
        public int? OverdueDays
        {
            get
            {
                if (!ReturnDate.HasValue && DateTime.Now > DueDate)
                {
                    return (DateTime.Now - DueDate).Days;
                }
                return null;
            }
        }

        public void MarkAsReturned()
        {
            ReturnDate = DateTime.Now;
        }


    }
}