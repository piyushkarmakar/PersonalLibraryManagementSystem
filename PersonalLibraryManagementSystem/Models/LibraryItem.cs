using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace PersonalLibraryManagementSystem.Models
{
    public abstract class LibraryItem
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyOrder(2)]
        public string Title { get; set; }
        [JsonPropertyOrder(3)]
        public DateTime DateAdded { get; set; }

        public abstract void DisplayDetails();
    }
}
