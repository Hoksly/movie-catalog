using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DataAccess;
using MovieCatalog.Models;

namespace FilmCatalog.Controllers
{
    public class FilmsController : Controller
    {
        private readonly FilmDbContext _context;

        public FilmsController(FilmDbContext context)
        {
            _context = context;
        }

        // GET: Films
        public IActionResult Index()
        {
            List<Film> films = _context.Films.Include(m => m.FilmCategories).ThenInclude(mc => mc.Category).ToList();
            if (films == null || !films.Any())
            {
                films = new List<Film>();
            }

            return View(films);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            ViewData["CategoryList"] = _context.Categories.ToList(); // Pass categories for dropdown
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to,
        // and validate the remaining properties if needed.
        [HttpPost]
        public IActionResult Create(Film Film, List<int> selectedCategories)
        {
            if (ModelState.IsValid)
            {
                Film.FilmCategories = new List<FilmCategory>();
                foreach (int categoryId in selectedCategories)
                {
                    Film.FilmCategories.Add(new FilmCategory { FilmId = Film.Id, CategoryId = categoryId });
                }

                _context.Films.Add(Film);
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    // Handle potential database update conflicts (e.g., unique constraint violations)
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                    ViewData["CategoryList"] = _context.Categories.ToList(); // Pass categories again
                    return View(Film);
                }
            }

            ViewData["CategoryList"] = _context.Categories.ToList(); // Pass categories again
            return View(Film);
        }

        // GET: Films/Edit/5
        public IActionResult Edit(int id)
        {
            var Film = _context.Films.Include(m => m.FilmCategories).ThenInclude(mc => mc.Category).FirstOrDefault(m => m.Id == id);
            if (Film == null)
            {
                return NotFound();
            }

            ViewData["SelectedCategoryIds"] = Film.FilmCategories.Select(mc => mc.CategoryId).ToList(); // Pre-select categories
            ViewData["CategoryList"] = _context.Categories.ToList();
            return View(Film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to,
        // and validate the remaining properties if needed.
        [HttpPost]
        public IActionResult Edit(int id, Film Film, List<int> selectedCategories)
        {
            if (id != Film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var FilmToUpdate = _context.Films.Include(m => m.FilmCategories).FirstOrDefault(m => m.Id == id);
                if (FilmToUpdate != null)
                {
                    FilmToUpdate.Name = Film.Name;
                    FilmToUpdate.Director = Film.Director;
                    FilmToUpdate.Release = Film.Release;

                    // Update Film categories (remove existing, add new)
                    FilmToUpdate.FilmCategories.Clear();
                    foreach (int categoryId in selectedCategories)
                    {
                        FilmToUpdate.FilmCategories.Add(new FilmCategory { FilmId = FilmToUpdate.Id, CategoryId = categoryId });
                    }

                    try
                    {
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateException /* ex */)
                    {
                        // Handle potential database update conflicts (e.g., unique constraint violations)
                        ModelState.AddModelError("", "Unable to save changes. Please try again.");
                    }
                }
            }

            ViewData["SelectedCategoryIds"] = selectedCategories; // Retain selected categories in case of validation errors
            ViewData["CategoryList"] = _context.Categories.ToList();
            return View(Film);
        }

        // GET: Films/Delete/5
        public IActionResult Delete(int id)
        {
            var Film = _context.Films.Find(id);
            if (Film == null)
            {
                return NotFound();
            }

            return View(Film);
        }

        public IActionResult Details(int id)
        {
            var film = _context.Films.Include(f => f.FilmCategories).ThenInclude(fc => fc.Category).FirstOrDefault(f => f.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }
    }

}
