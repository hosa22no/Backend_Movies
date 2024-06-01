using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MR_dw2.Models;

namespace MR_dw2.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
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

           

            // Configure the relationship between Movies and Reviews
            modelBuilder.Entity<Movies>()
               .HasMany(m => m.Reviews) 
               .WithOne(r => r.Movie) 
               .HasForeignKey(r => r.MovieId); 
        }

    
    }
}
