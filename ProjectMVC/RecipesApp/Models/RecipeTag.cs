using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesApp.Models
{
    public class RecipeTag
    {
        public int RecipeTagID { get; set; }
        public string TagName { get; set; }
    }
}
