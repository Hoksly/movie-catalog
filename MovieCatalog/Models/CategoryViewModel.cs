namespace MovieCatalog.Models
{
    public class CategoryViewModel(Category category)
    {
        public Category Category {get; set;} = category;
        public List<CategoryViewModel> SubCategories { get; set; } = new List<CategoryViewModel>();
        public List<Film> Films { get; set; } = new List<Film>();
        public int NestingLevel { get; set; } = 0;

        public void calculateNestingLevel()
        {
            foreach (var subCategory in SubCategories)
            {
                subCategory.calculateNestingLevel();
                NestingLevel = int.Max(NestingLevel, subCategory.NestingLevel + 1);
            }
            
        }
    }
}
