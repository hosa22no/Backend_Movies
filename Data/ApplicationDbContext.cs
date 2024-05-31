using Microsoft.EntityFrameworkCore;
using MR_dw2.Models;

namespace MR_dw2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        // Constructor that takes options as a parameter and passes it to the base class constructor    
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movies>().HasData(
                new Movies { Id = 1, Title = "The Shawshank Redemption", ReleaseYear = 1994, Description = "Cool movie from the 90s" },
                new Movies { Id = 2, Title = "The Godfather", ReleaseYear = 1972, Description = "Classic crime film" },
                new Movies { Id = 3, Title = "Pulp Fiction", ReleaseYear = 1994, Description = "Quentin Tarantino masterpiece" },
                new Movies { Id = 4, Title = "The Dark Knight", ReleaseYear = 2008, Description = "Epic superhero film" }
            );

        }
    }
}
