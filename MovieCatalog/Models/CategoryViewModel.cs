namespace MovieCatalog.Models
{
    public class CategoryViewModel(Category category)
    {
        public Category Category {get; set;} = category;
        public List<CategoryViewModel> SubCategories { get; set; } = new List<CategoryViewModel>();
        public int NestingLevel { get; set; } = 0;

        public void calculateNestingLevel()
        {
            if (Category.ParentCategoryId != null)
            {
                
                foreach (var subCategory in SubCategories)
                {
                    subCategory.calculateNestingLevel();
                    NestingLevel = int.Max(NestingLevel, subCategory.NestingLevel + 1);
                }
            }
        }
    }
}
