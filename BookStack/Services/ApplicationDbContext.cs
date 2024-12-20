using BookStack.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStack.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
    }
}
