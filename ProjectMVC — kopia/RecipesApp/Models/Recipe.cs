using System.Collections.Generic;
using RecipesApp.Models;

namespace RecipesApp.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        public string Name { get; set; }


        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        public string Instructions { get; set; }

        //Tagi w ielu przpisach wiele tagow w jednym przepisie
        public List<RecipeTag> Tags { get; set; } = new();

        public string PrepTime { get; set; }

        // z JWT 
        public int CreatedBy { get; set; }
    }
}
