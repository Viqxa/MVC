using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesApp.Models
{
    public class RecipeIngredient
    {
        // Composite key: RecipeId + IngredientId
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;

        public float WeightInGrams { get; set; }
        public float KcalPerPortion { get; set; }
    }
}
