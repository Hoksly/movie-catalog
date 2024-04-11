using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DataAccess;
using MovieCatalog.Models;

namespace MovieCatalog.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly FilmDbContext _context;

        public CategoriesController(FilmDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var parentCategories = await _context.Categories.Include(f => f.FilmCategories)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();

            List<CategoryViewModel> parentCategoryViewModels = new List<CategoryViewModel>();

            foreach (var category in parentCategories)
            {
                var parentCategoryViewModel = new CategoryViewModel(category);
                parentCategoryViewModel.SubCategories = await getSubCategories(category.Id);
                parentCategoryViewModel.calculateNestingLevel();
                parentCategoryViewModels.Add(parentCategoryViewModel);
            }

            return View(parentCategoryViewModels);
        }

        private async Task<List<CategoryViewModel>> getSubCategories(int categoryId)
        {
            var categories = await _context.Categories.Include(f => f.FilmCategories)
                .Where(c => c.ParentCategoryId == categoryId)
                .ToListAsync();

            List<CategoryViewModel> subCategories = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                var subCategory = new CategoryViewModel(category);
                subCategory.SubCategories = await getSubCategories(category.Id);
                subCategories.Add(subCategory);
            }
            return subCategories;
        }
        
    
    }
}