using Microsoft.EntityFrameworkCore;
using RecipesApp.Models;

namespace RecipesApp.Data
{
    public class RecipesContext : DbContext
    {
        public RecipesContext(DbContextOptions<RecipesContext> options) : base(options) { }

        public DbSet<Recipe> Recipes { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Ingredient> Ingredients { get; set; } = default!;
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = default!;
        public DbSet<RecipeTag> RecipeTags { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            
            modelBuilder.Entity<RecipeTag>().HasData(
                new RecipeTag { RecipeTagID = 1, TagName = "Breakfast" },
                new RecipeTag { RecipeTagID = 2, TagName = "Lunch" },
                new RecipeTag { RecipeTagID = 3, TagName = "Dinner" },
                new RecipeTag { RecipeTagID = 4, TagName = "Snack" },
                new RecipeTag { RecipeTagID = 5, TagName = "Vegan" },
                new RecipeTag { RecipeTagID = 6, TagName = "Vegetarian" },
                new RecipeTag { RecipeTagID = 7, TagName = "Gluten Free" },
                new RecipeTag { RecipeTagID = 8, TagName = "Keto" },
                new RecipeTag { RecipeTagID = 9, TagName = "Fast" }
            );
        }
    }
}
