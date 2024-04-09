class CategoryManager {
    constructor(filmId) {
        this.filmId = filmId;
        this.categoryList = document.getElementById('category-list');
        this.options = document.querySelector('.options');
        this.categoryList = document.getElementById('category-list');
        this.dropdownContainer = document.getElementById('multi-select-container'); // Assuming you have this in your HTML

        this.movieCategories = [];
        this.allCategories = [];
        
        this.update_button = document.getElementById('update-category');
        this.update_button.addEventListener('click', this.updateButtonClicked.bind(this));

    }
    renderPage() {
        this.getCategoriesForMovie(this.filmId).then(() => this.getAllCategories());

        
    }


    getAllCategories() {
        fetch('/api/MovieCategories/GetAllCategories')
            .then(response => response.json())
            .then(data => {
                this.allCategories = data;
                this.renderDropdown()
            });
      
 
        
    }
    async updateButtonClicked() {
        const checkboxes = document.querySelectorAll('.dropdown-menu input[type="checkbox"]:checked');

        var selectedCategories = [];
        checkboxes.forEach(checkbox => {
            selectedCategories.push(parseInt(checkbox.value));
        });

        // add to movieCategories
        for (let categoryId of selectedCategories) {
            if (!this.movieCategories.find(movieCategory => movieCategory.id === categoryId)) {
                let newCategory = this.allCategories.find(category => category.id === categoryId);
                await this.addCategoryToMovie(newCategory.id);
                this.movieCategories.push(newCategory);
            }
        }

        // remove from movieCategories
        for (let movieCategory of this.movieCategories) {
            if (!selectedCategories.includes(movieCategory.id)) {
                let category = this.allCategories.find(category => category.id === movieCategory.id);
                await this.removeCategoryFromMovie(category.id);
                this.movieCategories = this.movieCategories.filter(movieCategory => movieCategory.id !== category.id);
            }
        }

        this.renderMovieCategories();
    }

    renderDropdown() {
         this.dropdownContainer.innerHTML = ''; // Clear existing content

        const dropdownMenu = document.createElement('ul');
        dropdownMenu.classList.add('dropdown-menu');
        dropdownMenu.classList.add('dropdown-menu-scrollable');
        dropdownMenu.setAttribute('aria-labelledby', 'multiSelectDropdown');

        this.allCategories.forEach(category => {
            const li = document.createElement('li');
            const label = document.createElement('label');
            const checkbox = document.createElement('input');

            checkbox.type = 'checkbox';
            // if category is in movieCategories, check the box
            if (this.movieCategories.find(movieCategory => movieCategory.id === category.id)) {
                checkbox.checked = true;
            }
            checkbox.value = category.id;
            label.textContent = category.name;
            label.appendChild(checkbox);
            li.appendChild(label);
            dropdownMenu.appendChild(li);
        });

        const dropdownButton = document.createElement('button');
        dropdownButton.classList.add('btn', 'btn-success', 'dropdown-toggle');
        dropdownButton.type = 'button';
        dropdownButton.id = 'multiSelectDropdown';
        dropdownButton.dataset.bsToggle = 'dropdown';
        dropdownButton.innerText = 'Select Categories';

        this.dropdownContainer.appendChild(dropdownButton);
        this.dropdownContainer.appendChild(dropdownMenu);
    }

    getCategoriesForMovie(movieId) {
        return fetch(`/api/MovieCategories/GetCategoriesForMovie?movieId=${movieId}`)
            .then(response => response.json())
            .then(data => {
                this.movieCategories = data
                this.renderMovieCategories()
            });
    }
    
    renderMovieCategories() {
        this.categoryList.innerHTML = '';
        
        this.movieCategories.forEach(category => {
            var option = document.createElement('option');
            option.value = category.id;
            option.textContent = category.name;
            this.categoryList.appendChild(option);
        });
    }

    async addCategoryToMovie(categoryId) {
        const url = `/api/MovieCategories/AddCategoryToMovie?movieId=${this.filmId}&categoryId=${categoryId}`   ;
      

        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                // Category added successfully
                // Update UI or perform other actions if needed
            } else if (response.status === 400) {
                // Bad request (e.g., category already associated)
                const errorText = await response.text();
                console.error('Error adding category:', errorText);
            } else {
                // Unexpected error
                console.error('Error adding category:', response.statusText);
            }
        } catch (error) {
            console.error('Network or fetch error:', error);
        }
    }


    async removeCategoryFromMovie(categoryId) {
        const url = `/api/MovieCategories/RemoveCategoryFromMovie?movieId=${this.filmId}&categoryId=${categoryId}`;

        try {
            const response = await fetch(url, {
                method: 'DELETE'
            });

            if (response.ok) {
               } else if (response.status === 400) {
                // Bad request (e.g., category not associated)
                const errorText = await response.text();
                console.error('Error removing category:', errorText);
            } else {
                console.error('Error removing category:', response.statusText);
            }
        } catch (error) {
            console.error('Network or fetch error:', error);
        }
    }


    
}