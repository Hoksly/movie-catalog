using Microsoft.EntityFrameworkCore;

using MovieCatalog.Models;

namespace MovieCatalog.DataAccess
{
    public class FilmDbContext : DbContext
    {
        public DbSet<Film> Films { get; set; } // DbSet for Films table
        public DbSet<Category> Categories { get; set; } // DbSet for Categories table
        public DbSet<FilmCategory> FilmCategories { get; set; } // DbSet for FilmCategories table
        
        public FilmDbContext(DbContextOptions<FilmDbContext> options) 
            : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Film model
            modelBuilder.Entity<Film>()
                .ToTable("films")  // Optional table name mapping
                .HasKey(f => f.Id);  // Primary key configuration

            // Configure Category model
            modelBuilder.Entity<Category>()
                .ToTable("categories")  // Optional table name mapping
                .HasKey(c => c.Id);  // Primary key configuration

            // Configure FilmCategory model (many-to-many relationship)
            modelBuilder.Entity<FilmCategory>()
                .ToTable("film_categories")  // Optional table name mapping
                .HasKey(fc => new { fc.FilmId, fc.CategoryId }); // Composite primary key

            // Configure relationships
            modelBuilder.Entity<Film>()
                .HasMany(f => f.FilmCategories)
                .WithOne(fc => fc.Film)
                .HasForeignKey(fc => fc.FilmId);
          
            modelBuilder.Entity<Category>()
                .HasMany(c => c.FilmCategories)
                .WithOne(fc => fc.Category)
                .HasForeignKey(fc => fc.CategoryId);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          optionsBuilder.UseNpgsql("Host=localhost:5433;Database=movie-db;Username=postgres;Password=postgres;"); // Replace with your connection details

        }
        
    }
}
