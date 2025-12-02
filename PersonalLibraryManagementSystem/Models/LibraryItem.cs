using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibraryManagementSystem.Models
{
    public abstract class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public DateTime DateAdded { get; set; }

        public abstract void DisplayDetails();
    }
}
