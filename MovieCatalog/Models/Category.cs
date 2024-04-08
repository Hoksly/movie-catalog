using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieCatalog.Models
{
    [Table("categories")]   
    public class Category
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; } // Primary Key

        [StringLength(200)]
        [Column("name")]
        public string Name { get; set; }
        [Column("parent_category_id")]
        public int? ParentCategoryId { get; set; } // nullable foreign key

        public ICollection<FilmCategory> FilmCategories { get; set; } // Navigation property for many-to-many relationship
    }

}
