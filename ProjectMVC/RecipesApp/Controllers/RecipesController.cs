using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipesApp.Data;
using RecipesApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace RecipesApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RecipesContext _context;
        public RecipesController(RecipesContext context) => _context = context;

        // GET: Recipes1
        public async Task<IActionResult> Index()
        {
            var recipes = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Tags)
                .ToListAsync();
            return View(recipes);
        }

        // GET: Recipes1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Tags)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
            if (recipe == null) return NotFound();
            return View(recipe);
        }

        // GET: Recipes1/Create
        public IActionResult Create()
        {
            ViewBag.AllTags = new MultiSelectList(
                _context.RecipeTags, "RecipeTagID", "TagName");
            return View();
        }

        // POST: Recipes1/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Instructions,PrepTime")] Recipe recipe,
            string[] selectedIngredients,
            float[] ingredientWeights,
            float[] ingredientKcals,
            int[] selectedTags)
        {
            if (selectedIngredients == null || selectedIngredients.Length == 0 ||
    selectedIngredients.Any(i => string.IsNullOrWhiteSpace(i)))
            {
                ModelState.AddModelError("", "Each ingredient must have a name. Please fill in all ingredient rows.");
            }

            //walidacjiea składników 
            var count = selectedIngredients?.Length ?? 0;
            if (ingredientWeights == null || ingredientWeights.Length < count)
            {
                ModelState.AddModelError("",
                    "Please enter a weight for each ingredient.");
            }
            if (ingredientKcals == null || ingredientKcals.Length < count)
            {
                ModelState.AddModelError("",
                    "Calories weren’t calculated correctly. Try editing each ingredient’s weight.");
            }

            if (ModelState.IsValid)
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                recipe.CreatedBy = userId != null ? int.Parse(userId) : 0;


                //  RecipeIngredients
                recipe.RecipeIngredients = new List<RecipeIngredient>();
                for (int i = 0;  i < count; i++)
                {
                    var ingName = selectedIngredients[i];
                    var weight = ingredientWeights[i];
                    var kcal100 = ingredientKcals[i];

                    // robienie Ingredient
                    var ing = await _context.Ingredients
                        .FirstOrDefaultAsync(x => x.Name == ingName);
                    if (ing == null)
                    {
                        ing = new Ingredient { Name = ingName, KcalPer100g = kcal100 };
                        _context.Ingredients.Add(ing);
                        await _context.SaveChangesAsync();
                    }

                    recipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        IngredientId = ing.IngredientId,
                        WeightInGrams = weight,
                        KcalPerPortion = (float)Math.Round(kcal100, 2)

                    });
                }

                //  tags
                recipe.Tags = new List<RecipeTag>();
                foreach (var tagId in selectedTags ?? Array.Empty<int>())
                {
                    var tag = await _context.RecipeTags.FindAsync(tagId);
                    if (tag != null) recipe.Tags.Add(tag);
                }

                
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllTags = new MultiSelectList(
                _context.RecipeTags, "RecipeTagID", "TagName", selectedTags);
            return View(recipe);
        }

        // GET: Recipes1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Tags)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
            if (recipe == null) return NotFound();

            ViewBag.AllTags = new MultiSelectList(
                _context.RecipeTags, "RecipeTagID", "TagName",
                recipe.Tags.Select(t => t.RecipeTagID));

            ViewBag.AllIngredients = new MultiSelectList(
                _context.Ingredients, "IngredientId", "Name",
                recipe.RecipeIngredients.Select(ri => ri.IngredientId));

            return View(recipe);
        }

        // POST: Recipes1/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
     int id,
     [Bind("RecipeId,Name,Instructions,PrepTime,CreatedBy")] Recipe recipe,
     string[] selectedIngredients,          
     float[] ingredientWeights,
     float[] ingredientKcals,               
     int[] selectedTags)
        {
            if (id != recipe.RecipeId) return NotFound();

          
            if (selectedIngredients == null || selectedIngredients.Length == 0 ||
                selectedIngredients.Any(i => string.IsNullOrWhiteSpace(i)))
            {
                ModelState.AddModelError("", "Each ingredient must have a name. Please fill in all ingredient rows.");
            }

            if (ModelState.IsValid)
            {
                var existing = await _context.Recipes
                    .Include(r => r.Tags)
                    .Include(r => r.RecipeIngredients)
                    .FirstOrDefaultAsync(r => r.RecipeId == id);

                if (existing == null) return NotFound();


                existing.Name = recipe.Name;
                existing.Instructions = recipe.Instructions;
                existing.PrepTime = recipe.PrepTime;
                existing.CreatedBy = recipe.CreatedBy;

                // odwswieżenie tags
                existing.Tags.Clear();
                foreach (var tagId in selectedTags ?? Array.Empty<int>())
                {
                    var tag = await _context.RecipeTags.FindAsync(tagId);
                    if (tag != null) existing.Tags.Add(tag);
                }

 
                existing.RecipeIngredients.Clear();
                for (int i = 0; i < selectedIngredients.Length; i++)
                {
                    var ingName = selectedIngredients[i];
                    var weight = ingredientWeights.Length > i ? ingredientWeights[i] : 0;
                    var kcal100 = ingredientKcals.Length > i ? ingredientKcals[i] : 0;


                    var ing = await _context.Ingredients
                        .FirstOrDefaultAsync(x => x.Name == ingName);
                    if (ing == null)
                    {
                        ing = new Ingredient { Name = ingName, KcalPer100g = kcal100 };
                        _context.Ingredients.Add(ing);
                        await _context.SaveChangesAsync();
                    }
                    existing.RecipeIngredients.Add(new RecipeIngredient
                    {
                        IngredientId = ing.IngredientId,
                        WeightInGrams = weight,
                        KcalPerPortion = (float)Math.Round(kcal100, 2)//per 100g
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var count = selectedIngredients?.Length ?? 0;
            recipe.RecipeIngredients = new List<RecipeIngredient>(count);
            for (int i = 0; i < count; i++)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    Ingredient = new Ingredient { Name = selectedIngredients[i] },
                    WeightInGrams = ingredientWeights.Length > i ? ingredientWeights[i] : 0,
                    KcalPerPortion = ingredientKcals.Length > i ? (float)Math.Round(ingredientKcals[i], 2) : 0
                });
            }

            ViewBag.AllTags = new MultiSelectList(
                _context.RecipeTags, "RecipeTagID", "TagName", selectedTags);
            return View(recipe);
        }


        // GET: Recipes1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Tags)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
            if (recipe == null) return NotFound();
            return View(recipe);
        }

        // POST: Recipes1/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Tags)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
            if (recipe != null)
            {

                recipe.Tags.Clear();
                recipe.RecipeIngredients.Clear();
                await _context.SaveChangesAsync();

                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        //zalogowani widzą swoje przepisy
        [Authorize]
        public async Task<IActionResult> MyRecipes()
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var recipes = await _context.Recipes
                .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
                .Include(r => r.Tags)
                .Where(r => r.CreatedBy == userId) 
                .ToListAsync();
            return View("Index", recipes);
        }

        private bool RecipeExists(int id) =>
            _context.Recipes.Any(e => e.RecipeId == id);
    }
}
