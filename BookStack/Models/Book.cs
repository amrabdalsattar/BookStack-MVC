using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookStack.Models
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        [Precision(2)]
        public double Price{ get; set; }

        [MaxLength(100)]
        public string Category{ get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
