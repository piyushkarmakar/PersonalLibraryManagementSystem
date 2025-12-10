using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;



namespace PersonalLibraryManagementSystem.Models
{
    public abstract class LibraryItem
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(2)]
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }
        [JsonPropertyOrder(3)]
        public DateTime DateAdded { get; set; }

        public abstract void DisplayDetails();
    }
}
