using System.ComponentModel.DataAnnotations;

namespace BookStack.Models
{
    public class BookDto
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";
        [Required]
        public double Price { get; set; }

        [Required]
        public string Category { get; set; } = "";

        public IFormFile? ImageFile { get; set; }
    }
}

