using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalog.Models
{
    
[Table("film_categories")]   
public class FilmCategory
{
    [Key]
    [Column("Id")]
    public int Id { get; set; } // Primary Key (composite key)
    
    [Column("film_id")]
    public int FilmId { get; set; } // Foreign key to Films table

    [Column("category_id")]
    public int CategoryId { get; set; } // Foreign key to Categories table

    public Film Film { get; set; } // Navigation property for Film

    public Category Category { get; set; } // Navigation property for Category
}

}
