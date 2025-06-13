using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeApp.Data;
using RecipeApp.Models;
using RecipeApp.Services;

namespace RecipeApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly RecipeCalculator _calc;
        private readonly IFoodDataService _foodSvc;

        public RecipesController(
            ApplicationDbContext db,
            RecipeCalculator calc,
            IFoodDataService foodSvc)
        {
            _db = db;
            _calc = calc;
            _foodSvc = foodSvc;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _db.Recipes
                .Include(r => r.Tags)
                .ToListAsync();
            return View(list);
        }

        public IActionResult Create()
            => View(new RecipeEditViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(RecipeEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Calculate calories
            var ingPairs = vm.Ingredients
                .Where(i => i.FdcId.HasValue)
                .Select(i => (i.FdcId!.Value, i.WeightInGrams));
            var kcal = await _calc.CalculateTotalCaloriesAsync(ingPairs);

            // Map to EF entities
            var recipe = new Recipe
            {
                Name = vm.Name,
                Instructions = vm.Instructions,
                PrepTime = vm.PrepTime,
                TotalKcal = kcal
            };
            _db.Recipes.Add(recipe);
            await _db.SaveChangesAsync();

            // Tags
            foreach (var tag in vm.SelectedTags)
                _db.RecipeTags.Add(new RecipeTag
                {
                    RecipeId = recipe.Id,
                    Tag = tag
                });
            await _db.SaveChangesAsync();

            // Ingredients
            foreach (var ing in vm.Ingredients.Where(i => i.FdcId.HasValue))
                _db.RecipeIngredients.Add(new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    FdcId = ing.FdcId!.Value,
                    Description = ing.Description,
                    WeightInGrams = ing.WeightInGrams
                });
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // AJAX autocomplete
        [HttpGet]
        public async Task<IActionResult> SearchFood(string term)
        {
            var foods = await _foodSvc.SearchAsync(term);
            var results = foods.Select(f => new {
                id = f.FdcId,
                label = f.Description
            });
            return Json(results);
        }
    }
}
