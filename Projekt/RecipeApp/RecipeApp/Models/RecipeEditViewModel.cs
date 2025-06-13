using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeApp.Models
{
    public class RecipeEditViewModel
    {
        public int? Id { get; set; }

        [Required] public string Name { get; set; }
        public string Instructions { get; set; }

        [Display(Name = "Prep Time (min)")]
        [Range(0, 1440)]
        public int PrepTime { get; set; }

        public List<IngredientInput> Ingredients
        { get; set; } = new() { new() };

        [Display(Name = "Tags")]
        public List<string> SelectedTags { get; set; } = new();

        public IEnumerable<SelectListItem> TagOptions { get; }
            = new List<SelectListItem>
            {
                new("sniadanie","sniadanie"),
                new("obiad","obiad"),
                new("kolacja","kolacja"),
                new("wege","wege"),
                new("wegan","wegan"),
                new("bez orzechów","bez orzechów"),
                new("szybkie","szybkie"),
            };
    }
}