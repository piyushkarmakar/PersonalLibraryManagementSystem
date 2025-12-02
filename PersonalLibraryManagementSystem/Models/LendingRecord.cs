using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibraryManagementSystem.Models
{
    public class LendingRecord
    {


        public int BookId { get; set; }
        public string BookName { get; set; }   
        public string FriendName { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }


        public void MarkAsReturned()
        {
            ReturnDate = DateTime.Now;
        }


    }
}