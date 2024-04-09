using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DataAccess;
using MovieCatalog.Models;

namespace MovieCatalog.Controllers
{
    [Route("api/[controller]")] 
    public class MovieCategoriesController : ControllerBase 
    {
        private readonly FilmDbContext _dbContext; 

        public MovieCategoriesController(FilmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var categories = _dbContext.Categories
                .Select(c => new {  // Project into an anonymous type for JSON serialization
                    id = c.Id,
                    name = c.Name
                })
                .ToList();

            return Ok(categories);
        }

        [HttpGet("GetCategoriesForMovie")] 
        public IActionResult GetCategoriesForMovie(int movieId) 
        {
            // Assuming your DbContext has DbSets for 'Movie' and 'Category'
            var categories = _dbContext.Categories
                .Where(c => c.FilmCategories.Any(m => m.FilmId == movieId)) // Filter Categories
                .Select(c => new {  // Project into an anonymous type for JSON serialization
                    id = c.Id,
                    name = c.Name
                })
                .ToList();

            return Ok(categories);           
        }


        [HttpPost("AddCategoryToMovie")] 
        public IActionResult AddCategoryToMovie(int movieId, int categoryId) 
        { 
            var movie = _dbContext.Films
                .Include(f => f.FilmCategories)  // Eager load FilmCategories
                .SingleOrDefault(f => f.Id == movieId);
            
                if (movie == null) 
                {
                    return NotFound("Movie not found"); 
                }

                var category = _dbContext.Categories.Find(categoryId);
                if (category == null)
                {
                    return NotFound("Category not found");
                }

                if (movie.FilmCategories.Any(fc => fc.CategoryId == categoryId))
                {
                    return BadRequest("Movie is already associated with this category");
                }


                movie.FilmCategories.Add(new FilmCategory { 
                    FilmId = movieId, 
                    CategoryId = categoryId 
                });


                _dbContext.SaveChanges(); 
                return Ok(); 
      
        }
        
        [HttpDelete("RemoveCategoryFromMovie")]
        public IActionResult RemoveCategoryFromMovie(int movieId, int categoryId)
        {
            try
            {
                var movie = _dbContext.Films
                    .Include(f => f.FilmCategories)  // Eager load FilmCategories
                    .SingleOrDefault(f => f.Id == movieId);

                if (movie == null)
                {
                    return NotFound("Movie not found");
                }

                var category = _dbContext.Categories.Find(categoryId);
                if (category == null)
                {
                    return NotFound("Category not found");
                }

                var filmCategory = movie.FilmCategories.FirstOrDefault(fc => fc.CategoryId == categoryId);
                if (filmCategory == null)
                {
                    return BadRequest("Movie is not associated with this category");
                }

                movie.FilmCategories.Remove(filmCategory);

                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while removing the category");
            }
        }
    }
    
}
