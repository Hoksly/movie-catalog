class MovieCategoryManager {
    constructor(movieId, categorySelectorId, apiUrl) {
        this.movieId = movieId;
        this.categorySelector = $(categorySelectorId); // assuming jQuery
        this.apiUrl = apiUrl;

        this.initializeSelector();
        this.loadCurrentCategories();
    }

    initializeSelector() {
        // Integrate a third-party library like Select2 or Choices.js
        this.categorySelector.select2({
            // Configure the library with options like multi-select, search, etc.
        });

        // Event listeners for adding/removing categories (tied to your library)
        this.categorySelector.on('select2:select', (event) => this.addCategory(event.params.data.id));
        this.categorySelector.on('select2:unselect', (event) => this.removeCategory(event.params.data.id));
    }

    loadCurrentCategories() {
        $.get(`${this.apiUrl}/GetCategoriesForMovie?movieId=${this.movieId}`)
            .done((categories) => {
                // Pre-select current categories in the selector 
            })
            .fail((error) => console.error("Error loading categories", error));
    }

    addCategory(categoryId) {
        $.post(`${this.apiUrl}/AddCategoryToMovie`, { movieId: this.movieId, categoryId: categoryId })
            .done(() => console.log("Category added"))
            .fail((error) => console.error("Error adding category", error));
    }

    removeCategory(categoryId) {
        // Similar to addCategory, but a different API endpoint
    }
}
