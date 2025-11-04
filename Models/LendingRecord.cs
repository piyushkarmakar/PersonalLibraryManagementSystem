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
        public required string FriendName { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }


        public void MarkAsReturned()
        {
            ReturnDate = DateTime.Now;
        }

        public int CalculateOverdueDays()
        {
            if (!ReturnDate.HasValue)
                return 0;

            int overdueDays = (ReturnDate.Value - DueDate).Days;
            return overdueDays > 0 ? overdueDays : 0;

        }
        public bool IsOverdue
        {
            get
            {
                if (ReturnDate.HasValue)
                {
                    return ReturnDate.Value > DueDate;
                }
                return false;
            }
        }
    }
}