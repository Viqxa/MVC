using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeApp.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Instructions { get; set; } = string.Empty;

        [Display(Name = "Prep Time (min)")]
        public int PrepTime { get; set; }

        public double TotalKcal { get; set; }

        public ICollection<RecipeIngredient> Ingredients { get; set; }
            = new List<RecipeIngredient>();

        public ICollection<RecipeTag> Tags { get; set; }
            = new List<RecipeTag>();
    }
}
