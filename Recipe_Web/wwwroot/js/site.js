////let recipes = [];
////let currentCategory = 'veg';
////let savedRecipes = JSON.parse(localStorage.getItem('savedRecipes')) || [];
////let favoriteRecipes = JSON.parse(localStorage.getItem('favoriteRecipes')) || [];
////let myRecipes = JSON.parse(localStorage.getItem('myRecipes')) || [];

////$(document).ready(function () {
////    fetchRecipes();

////    function fetchRecipes() {
////        $.ajax({
////            url: '/json/recipes.json',
////            type: 'GET',
////            dataType: 'json',
////            success: function (data) {
////                recipes = data;
////                renderPage(currentCategory);
////            },
////            error: function () {
////                $('#mainContent').html('<p>Sorry, unable to load recipes right now.</p>');
////            }
////        });
////    }

//    function capitalize(str) {
//        return str.charAt(0).toUpperCase() + str.slice(1);
//    }

//    function recipeCardHTML(recipe) {
//        return `
//      <div class="card" id="recipe-${recipe.id}">
//        <img src="${recipe.img}" alt="${recipe.name}" />
//        <h3>${recipe.name}</h3>
//        <p>${recipe.description || ''}</p>
//        <p class="rating">⭐ ${'★'.repeat(recipe.rating)}${'☆'.repeat(5 - recipe.rating)}</p>
//        <div class="details-section">
//          <h4>Ingredients:</h4>
//          <ul>${recipe.ingredients.map(i => `<li>${i}</li>`).join('')}</ul>
//          <h4>Instructions:</h4>
//          <ol>${recipe.instructions.map(i => `<li>${i}</li>`).join('')}</ol>
//          ${recipe.nutrition ? `
//          <h4>Nutrition:</h4>
//          <ul>
//            <li>Calories: ${recipe.nutrition.calories}</li>
//            <li>Protein: ${recipe.nutrition.protein}</li>
//            <li>Carbs: ${recipe.nutrition.carbs}</li>
//            <li>Fat: ${recipe.nutrition.fat}</li>
//          </ul>` : ''}
//        </div>
//        <div class="bottom-buttons">
//          <button class="action-btn" onclick="toggleSave(${recipe.id})">${savedRecipes.includes(recipe.id) ? 'Unsave' : 'Save'}</button>
//          <button class="action-btn" onclick="toggleFavorite(${recipe.id})">${favoriteRecipes.includes(recipe.id) ? 'Unfavorite' : 'Favorite'}</button>
//        </div>
//      </div>
//    `;
//    }

//    window.renderPage = function (category) {
//        currentCategory = category;
//        const filtered = recipes.filter(r => r.category === category);
//        $('#mainContent').html(`
//      <section class="section">
//        <h2>${capitalize(category)} Recipes</h2>
//        <div class="recipe-grid">
//          ${filtered.map(recipeCardHTML).join('')}
//        </div>
//      </section>
//    `);
//    };

//    window.renderSaved = function () {
//        currentCategory = 'saved';
//        const savedList = recipes.filter(r => savedRecipes.includes(r.id));
//        $('#mainContent').html(`
//      <section class="section">
//        <h2>Saved Recipes</h2>
//        ${savedList.length === 0
//                ? '<p class="empty-msg">You have no saved recipes yet.</p>'
//                : `<div class="recipe-grid">${savedList.map(recipeCardHTML).join('')}</div>`}
//      </section>
//    `);
//    };

//    window.renderFavorites = function () {
//        currentCategory = 'favorites';
//        const favList = recipes.filter(r => favoriteRecipes.includes(r.id));
//        $('#mainContent').html(`
//      <section class="section">
//        <h2>Favorite Recipes</h2>
//        ${favList.length === 0
//                ? '<p class="empty-msg">You have no favorite recipes yet.</p>'
//                : `<div class="recipe-grid">${favList.map(recipeCardHTML).join('')}</div>`}
//      </section>
//    `);
//    };

//    window.toggleSave = function (id) {
//        savedRecipes = savedRecipes.includes(id)
//            ? savedRecipes.filter(rid => rid !== id)
//            : [...savedRecipes, id];
//        localStorage.setItem('savedRecipes', JSON.stringify(savedRecipes));
//        refreshCurrentView();
//    };

//    window.toggleFavorite = function (id) {
//        favoriteRecipes = favoriteRecipes.includes(id)
//            ? favoriteRecipes.filter(rid => rid !== id)
//            : [...favoriteRecipes, id];
//        localStorage.setItem('favoriteRecipes', JSON.stringify(favoriteRecipes));
//        refreshCurrentView();
//    };

//    function refreshCurrentView() {
//        if (currentCategory === 'saved') {
//            renderSaved();
//        } else if (currentCategory === 'favorites') {
//            renderFavorites();
//        } else {
//            renderPage(currentCategory);
//        }
//    }

//    window.toggleTheme = function () {
//        $('body').toggleClass('dark-theme');
//    };


////window.toggleTheme = function () {
////    $('body').toggleClass('dark-theme');

////    // Save the theme preference to localStorage
////    if ($('body').hasClass('dark-theme')) {
////        localStorage.setItem('theme', 'dark');
////    } else {
////        localStorage.setItem('theme', 'light');
////    }
////};

////// On page load: apply the saved theme
////$(document).ready(function () {
////    if (localStorage.getItem('theme') === 'dark') {
////        $('body').addClass('dark-theme');
////    }
////});

//    window.renderMyRecipes = function () {
//        $('#mainContent').html(`
//      <section class="section">
//        <h2>My Recipes</h2>
//        <form id="myRecipeForm" enctype="multipart/form-data">
//          <input type="text" id="recipeName" placeholder="Recipe Name" required><br>
//          <input type="file" id="recipeImage" accept=".png,.jpg,.jpeg,.webp" required><br>
//          <select id="recipeCategory">
//            <option value="veg">Veg</option>
//            <option value="nonveg">Non-Veg</option>
//            <option value="beverages">Beverages</option>
//          </select><br>
//          <textarea id="recipeIngredients" placeholder="Ingredients (comma-separated)" required></textarea><br>
//          <textarea id="recipeInstructions" placeholder="Instructions (comma-separated)" required></textarea><br>
//          <input type="number" id="recipeRating" placeholdter="Rating (1 to 5)" min="1" max="5" required><br>
//          <button type="submit">Add Recipe</button>
//        </form>
//        <div class="recipe-grid">
//          ${myRecipes.map((r, index) => `
//            <div class="card">
//              <img src="${r.img}" alt="${r.name}">
//              <h3>${r.name}</h3>
//              <p>Category: ${capitalize(r.category)}</p>
//              <p class="rating">⭐ ${'★'.repeat(r.rating)}${'☆'.repeat(5 - r.rating)}</p>
//              <div class="details-section">
//                <h4>Ingredients:</h4>
//                <ul>${r.ingredients.map(i => `<li>${i}</li>`).join('')}</ul>
//                <h4>Instructions:</h4>
//                <ol>${r.instructions.map(i => `<li>${i}</li>`).join('')}</ol>
//              </div>
//              <div class="bottom-buttons">
//                <button class="action-btn" onclick="deleteMyRecipe(${index})">Delete</button>
//              </div>
//            </div>
//          `).join('')}
//        </div>
//      </section>
//    `);

//        $('#myRecipeForm').on('submit', function (e) {
//            e.preventDefault();
//            const name = $('#recipeName').val().trim();
//            const file = $('#recipeImage')[0].files[0];

//            if (!file) {
//                alert('Please upload an image.');
//                return;
//            }

//            const validTypes = ['image/png', 'image/jpeg', 'image/webp'];
//            if (!validTypes.includes(file.type)) {
//                alert('Only PNG and JPG/JPEG and WEBP images are allowed.');
//                return;
//            }

//            const reader = new FileReader();
//            reader.onload = function (event) {
//                const newRecipe = {
//                    name,
//                    img: event.target.result,
//                    category: $('#recipeCategory').val(),
//                    ingredients: $('#recipeIngredients').val().split(',').map(i => i.trim()),
//                    instructions: $('#recipeInstructions').val().split(',').map(i => i.trim()),
//                    rating: parseInt($('#recipeRating').val())
//                };

//                myRecipes.push(newRecipe);
//                localStorage.setItem('myRecipes', JSON.stringify(myRecipes));
//                renderMyRecipes();
//            };

//            reader.readAsDataURL(file);
//        });
//    };

//    window.deleteMyRecipe = function (index) {
//        myRecipes.splice(index, 1);
//        localStorage.setItem('myRecipes', JSON.stringify(myRecipes));
//        renderMyRecipes();
//    };
//});



