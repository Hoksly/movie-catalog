using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalog.Models
{
    [Table("films")]   
    public class Film
    {
        public int Id { get; set; } // Primary Key

        [StringLength(200)] // Limit name length to 200 characters
        [Column("name")]
        public string Name { get; set; }

        [StringLength(200)] // Limit director length to 200 characters
        [Column("director")]
        public string Director { get; set; }

        [Column("release")]
        public DateTime Release { get; set; }
        public ICollection<FilmCategory> FilmCategories { get; set; }
        
    }
    
}
